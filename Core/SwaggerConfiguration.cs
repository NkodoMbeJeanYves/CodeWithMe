using System.Reflection;
using Microsoft.OpenApi.Models;

namespace CodeWithMe.Core;

public static class SwaggerConfiguration
{
    public static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(it =>
        {
            it.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "EDU Platform API",
                Description =
                    "API multi-tenant de gestion scolaire et universitaire — contrat EDU Platform v1. " +
                    "Réponses normalisées (data/meta/links), erreurs RFC 7807 simplifiées, " +
                    "header X-Tenant-Id obligatoire hors /v1/auth/*.",
                Contact = new OpenApiContact
                {
                    Name = "EDU Platform Backend",
                    Email = "support@edu.example.com"
                }
            });

            it.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT — exemple : \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            it.AddSecurityDefinition("TenantId", new OpenApiSecurityScheme
            {
                Description = "Tenant courant — X-Tenant-Id (uuid).",
                Name = "X-Tenant-Id",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            it.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            });

            it.AddServer(new OpenApiServer
            {
                Url = "http://localhost:5228",
                Description = "Local dev (mock URL contractuelle : http://localhost:4010/v1)"
            });
            it.AddServer(new OpenApiServer
            {
                Url = "https://api.edu.example.com/v1",
                Description = "Production (target contractuelle)"
            });

            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(xmlPath)) it.IncludeXmlComments(xmlPath);
        });
    }
}
