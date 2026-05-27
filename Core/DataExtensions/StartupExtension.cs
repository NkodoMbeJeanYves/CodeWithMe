using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using CodeWithMe.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace CodeWithMe.Core.DataExtensions;

public static class StartupExtension
{
    public static WebApplicationBuilder InitializeApplicationServices(this WebApplicationBuilder builder)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();
        // Add services to the container.
        var jwtConfig = new JwtConfig();
        builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);

        // Injection des config en Singleton
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
        builder.Services.Configure<DaySettings>(builder.Configuration.GetSection("DaySettings"));

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Contrat EDU v1 : JSON camelCase, enums en snake_case (super_admin, on_leave, ...),
                // omission des null pour rester proche des exemples du contrat.
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        //Register All validators of this Assembly Or Project
        builder.Services.AddValidatorsFromAssemblyContaining<SchoolDtoValidator>(ServiceLifetime.Transient);

        // Stop FluentValidation on the first failure, it is useful when you define several
        // rules for a property and you want to avoid multiple error messages for the same property
        ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

        //builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, include)
        builder.Services.AddScoped(TokenService => new TokenService(jwtConfig));
        builder.Services.AddSingleton<FakeService>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // Multi-tenant : ITenantContext est hydraté par TenantResolutionMiddleware
        // et lu par le filtre global EF de ApiContext.
        builder.Services.AddScoped<ITenantContext, TenantContext>();

        // Identity
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApiContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        // adding swagger configuration with JWT support
        builder.AddSwaggerConfiguration();
        builder.AddJwtConfiguration(jwtConfig);

        return builder;
    }

    /*
         * this method extends the WebApplication class to include a method for migrating the database. 
         * It creates a scope for the application's services, retrieves the ApiContext from the service provider, 
         * and calls the Migrate method on the database to apply any pending migrations. 
         * This ensures that the database schema is up to date with the application's data model when the application starts.
         */
    /// <summary>
    /// Applique les migrations EF puis seed les comptes mock du contrat (section 8.3).
    /// ⚠️ À activer dans Program.cs seulement APRÈS avoir généré une migration couvrant
    /// le nouveau schéma (multi-tenant + academic + admin + finance + communication) :
    ///     dotnet tool install --global dotnet-ef
    ///     dotnet ef migrations add ContractV1Schema
    /// Sinon EF tentera d'appliquer un schéma incomplet et crashera au démarrage.
    /// </summary>
    public static async Task MigrateDbAsync(this WebApplication app, bool shouldApplySeed = true)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApiContext>();
        await dbContext.Database.MigrateAsync();

        if (shouldApplySeed)
        {
            await Seeder.SeedContractAccountsAsync(app.Services);
        }
    }

    public static WebApplication establishConnection(this WebApplicationBuilder builder)
    {
        // Add database context before building the app
        // ref: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql#readme-body-tab
        var connectionString = builder.Configuration.GetConnectionString("ApiConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 4, 3));

        /**
         * ApiContext has a scoped service lifetime because:
         * 1. It ensures that a new instance of ApiContext is created per request, which is important for managing database connections and ensuring thread safety.
         * 2. It allows for better performance and resource management by reusing the same instance of ApiContext within a single request, while still providing isolation between different requests.
         * 3. It ensures that the database context is properly disposed of at the end of each request, preventing potential memory leaks and ensuring that database connections are released back to the connection pool in a timely manner.
         * 4. ApiContext is not thread-safe. Scope avoids to concurrency issues that could arise if multiple requests were to share the same instance of ApiContext.
         * 5. Makes it easier to manage transactions and ensure data consistency.
         */
        builder.Services.AddDbContext<ApiContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );

        // Enabling Logger
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        return builder.Build();
    }

    public static WebApplicationBuilder InitializeRateLimiterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            // Politique unique alignée au contrat (section 10) :
            // - 100 req/min/user en lecture (GET/HEAD)
            // - 30 req/min/user en écriture (POST/PUT/PATCH/DELETE)
            options.AddPolicy("GlobalPolicy", context =>
            {
                var isRead = HttpMethods.IsGet(context.Request.Method)
                          || HttpMethods.IsHead(context.Request.Method);
                var limit = isRead ? 100 : 30;
                var partitionKey = context.User.Identity?.Name
                                   ?? context.Connection.RemoteIpAddress?.ToString()
                                   ?? context.Request.Headers.Host.ToString();

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: $"{(isRead ? "r" : "w")}:{partitionKey}",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = limit,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
            });

            // Réponse 429 alignée RFC 7807 du contrat.
            options.OnRejected = async (ctx, ct) =>
            {
                ctx.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                ctx.HttpContext.Response.ContentType = "application/json";
                var payload = new Core.Models.Envelope.ApiErrorResponse(
                    new[] { new Core.Models.Envelope.ApiError("RATE_LIMITED", "Too many requests.") },
                    System.Diagnostics.Activity.Current?.TraceId.ToString() ?? ctx.HttpContext.TraceIdentifier,
                    DateTime.UtcNow,
                    ctx.HttpContext.Request.Path);
                await ctx.HttpContext.Response.WriteAsJsonAsync(payload, ct);
            };
        });
        return builder;
    }
}
