using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Models;
using CodeWithMe.Middlewares;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

// Initialize Application Services
// Establish Database connection
var app = builder.InitializeApplicationServices().InitializeRateLimiterServices().establishConnection();

//app.UseRateLimiter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //    options.RoutePrefix = string.Empty;
    //});
    app.MapSwagger("/openapi/{documentName}.json");
    //app.MapScalarApiReference("/api-docs");
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("CodeWithMe API")
               .WithClassicLayout()
               .ForceDarkMode()
               .HideSearch()
               .ShowOperationId()
               .ExpandAllTags()
               .SortTagsAlphabetically()
               .SortOperationsByMethod()
               .PreserveSchemaPropertyOrder()
               .AddApiKeyAuthentication("Authorization", _ => { /* configure ScalarApiKeySecurityScheme here if needed */ })
               .AddServer("http://localhost:5228", "Dev");
    });
}

// Run database migrations, seed at startup
// ⚠️ À activer SEULEMENT après avoir généré la migration contract-v1 :
//     dotnet tool install --global dotnet-ef
//     dotnet ef migrations add ContractV1Schema
//     dotnet ef database update
// Une fois la migration disponible, décommenter la ligne ci-dessous pour
// appliquer la migration ET seeder les 8 comptes mock du contrat (section 8.3).
// if (app.Environment.IsDevelopment()) await app.MigrateDbAsync(shouldApplySeed: true);

// 1. Gestion globale des exceptions
app.UseExceptionHandler();

// 2. Middleware qui prépare le JWT (si tu forces un token par défaut)
app.UseMiddleware<DefaultJwtMiddleware>();

// 3. Authentification & Autorisation
app.UseAuthentication();
app.UseAuthorization();

// Place UseRateLimiter() after routing and authentication so policies can evaluate user identity/IP correctly.
app.UseRateLimiter();

// 4. Vérification de révocation (après que User soit construit)
app.UseMiddleware<JwtRevocationMiddleware>();

// 5. Résolution multi-tenant (X-Tenant-Id vs claim tenantId JWT)
app.UseMiddleware<TenantResolutionMiddleware>();

// 6. Validation DTO
app.UseMiddleware<ValidationMiddleware>();

// 6. Endpoints
app.MapControllers().RequireRateLimiting("GlobalPolicy");
app.MapIdentityApi<User>();


app.Run();
