using CodeWithMe.Core;
using CodeWithMe.EndPoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddValidation(); // This should be in Program.cs
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
   static it =>
   {
       it.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
       {
           Version = "v1",
           Title = "REDACTED_PROJECT_NAME API",
           Description = "An ASP.NET Core Web API for managing REDACTED_PROJECT_NAME.",
           Contact = new Microsoft.OpenApi.Models.OpenApiContact
           {
               Name = "Nkodo Mbe Jean Yves",
               Email = "nkodomjy@gmail.com",
           }
       });
   }
   );

//Enabling Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add database context before building the app
// ref: https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql#readme-body-tab
var connectionString = builder.Configuration.GetConnectionString("ApiConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 4, 3));
// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<ApiContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", static () => "Hello World!");

app.MapGameEndPoints();

// Run database migrations at startup
app.MigrateDb();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
