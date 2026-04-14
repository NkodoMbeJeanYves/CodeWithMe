using CodeWithMe.Core;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using CodeWithMe.EndPoints;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var jwtConfig = new JwtConfig();
builder.Services.AddSingleton<JwtConfig>();
builder.Configuration.GetSection("JwtConfig").Bind(jwtConfig);

builder.Services.AddControllers();
//builder.Services.AddScoped<FakeService>();
builder.Services.AddSingleton<FakeService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
   static it =>
   {
       it.SwaggerDoc("v1", new OpenApiInfo
       {
           Version = "v1",
           Title = "CodeWithMe API",
           Description = "An ASP.NET Core Web API for managing REDACTED_PROJECT_NAME.",
           Contact = new OpenApiContact
           {
               Name = "Nkodo Mbe Jean Yves",
               Email = "nkodomjy@gmail.com",
           }
       });
   }
   );

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();

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

app.MapGet("/", static () => "Hello World!").RequireAuthorization();

app.MapGameEndPoints();

app.MapSubjectEndPoints();

// Run database migrations, seed at startup
// Ensure Database is populated and up-to-date with the latest schema changes
// app.MigrateDb();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/token", () => {
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
