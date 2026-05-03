using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using CodeWithMe.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMe.Core.DataExtensions;

public static class WebApplicationBuilderExtension
{
    public static void InitializeApplicationServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        var jwtConfig = new JwtConfig();
        builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);

        // Injection des config en Singleton
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
        builder.Services.Configure<DaySettings>(builder.Configuration.GetSection("DaySettings"));

        builder.Services.AddControllers();

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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Identity
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApiContext>()
            .AddApiEndpoints()
            .AddDefaultTokenProviders();

        // adding swagger configuration with JWT support
        builder.AddSwaggerConfiguration();
        builder.AddJwtConfiguration(jwtConfig);
    }
}
