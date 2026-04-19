using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using CodeWithMe.Middlewares;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
var jwtConfig = new JwtConfig();
builder.Services.AddSingleton<JwtConfig>();
builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);

builder.Services.AddControllers();

//Register All validators of this Assembly Or Project
builder.Services.AddValidatorsFromAssemblyContaining<SchoolDtoValidator>(ServiceLifetime.Transient);

//builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, include)
builder.Services.AddSingleton<FakeService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// adding swagger configuration with JWT support
builder.AddSwaggerConfiguration();
builder.AddJwtConfiguration(jwtConfig);

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

app.UseAuthentication();
app.UseAuthorization();


// Positionner avant MapControllers
// GlobalExceptionMiddleware
app.UseExceptionHandler();
// Chaque requête qui contient un DTO validé par FluentValidation sera interceptée.
// Si la validation échoue, le middleware renvoie directement un ValidationProblemDetails avec la route et la méthode.
app.UseMiddleware<ValidationMiddleware>();


app.MapGet("/token", () =>
{
    var issuer = jwtConfig.Issuer;
    var audience = jwtConfig.Audience;
    var secretKey = jwtConfig.SecretKey;
    var expirationMinutes = 60;
    var tokenExpirytimeStamp = DateTime.UtcNow.AddMinutes(expirationMinutes);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
                    new Claim(ClaimTypes.Name, "UserName")
                }),
        Expires = tokenExpirytimeStamp,
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var accessToken = tokenHandler.WriteToken(securityToken);
    return Results.Ok(accessToken);
});

app.MapControllers();

app.Run();
