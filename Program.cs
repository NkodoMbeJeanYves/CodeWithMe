using CodeWithMe.Core;
using CodeWithMe.EndPoints;
using CodeWithMe.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddScoped<FakeService>();
builder.Services.AddSingleton<FakeService>();
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

app.MapGet("/", static () => "Hello World!");

app.MapGameEndPoints();

app.MapSubjectEndPoints();

// Run database migrations, seed at startup
// Ensure Database is populated and up-to-date with the latest schema changes
// app.MigrateDb();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
