using Microsoft.OpenApi.Models;

namespace CodeWithMe.Core
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(
   static it =>
   {
       it.SwaggerDoc("v1", new OpenApiInfo
       {
           Version = "v1",
           Title = "CodeWithMe API",
           Description = "An ASP.NET Core Web API for managing CodeWithMe.",
           Contact = new OpenApiContact
           {
               Name = "Nkodo Mbe Jean Yves",
               Email = "nkodomjy@gmail.com",
           }
       });
       // Add JWT bearer definition
       it.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
       {
           Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
           Name = "Authorization",
           In = ParameterLocation.Header,
           Type = SecuritySchemeType.Http,
           Scheme = "Bearer"

       });
       // Require token for all endpoints unless AllowAnonymous
       it.AddSecurityRequirement(new OpenApiSecurityRequirement()
       {
           {
               new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                   {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                   },
                   Scheme = "Bearer",
                   Name = "Bearer",
                   In = ParameterLocation.Header,
               },
               new List<string>()
           }
       });

       it.AddServer(new OpenApiServer
       {
           Url = "http://localhost:5228",
           Description = "Local development server"
       });
   }
   );
        }
    }
}
