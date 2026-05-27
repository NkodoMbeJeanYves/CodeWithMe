using System.Text;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CodeWithMe.Core.DataExtensions;

public static class JwtConfiguration
{
    public static void AddJwtConfiguration(this WebApplicationBuilder builder, JwtConfig jwtSettings)
    {
        var secretKey = jwtSettings.SecretKey;
        var issuer = jwtSettings.Issuer;
        var audience = jwtSettings.Audience;

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                NameClaimType = System.Security.Claims.ClaimTypes.Name
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            // Policies par niveau d'autorité (contrat section 7.3).
            // Le claim "authorityLevel" est positionné par TokenService.GenerateContractJwt.
            foreach (AuthorityLevel level in Enum.GetValues<AuthorityLevel>())
            {
                var snake = System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(level.ToString());
                options.AddPolicy($"Authority.{level}", p =>
                    p.RequireAuthenticatedUser()
                     .RequireClaim("authorityLevel", snake));
            }

            // Policies fines par rôle (utilisable via [Authorize(Policy = "Role.Directeur")]).
            foreach (Role role in Enum.GetValues<Role>())
            {
                var snake = System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(role.ToString());
                options.AddPolicy($"Role.{role}", p =>
                    p.RequireAuthenticatedUser()
                     .RequireClaim("role", snake));
            }
        });
    }
}
