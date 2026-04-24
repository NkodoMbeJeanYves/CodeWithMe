using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Middlewares;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Initialize Application Services
builder.InitializeApplicationServices();

// Establish Database connection
builder.establishConnection();

// Enabling Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Run database migrations, seed at startup
// Ensure Database is populated and up-to-date with the latest schema changes
// app.MigrateDb();

app.UseHttpsRedirection();
// Set Default JWT Token validation for all endpoints. This middleware will set a valid JWT token in the Authorization header of incoming requests.
app.UseMiddleware<DefaultJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


// Positionner avant MapControllers
// GlobalExceptionMiddleware
app.UseExceptionHandler();
// Chaque requête qui contient un DTO validé par FluentValidation sera interceptée.
// Si la validation échoue, le middleware renvoie directement un ValidationProblemDetails avec la route et la méthode.
app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();
