
using CodeWithMe.Core.Dtos.Auth;
using CodeWithMe.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeWithMe.Core.Services;

public class TokenService
{
    private readonly JwtConfig _jwtConfig;

    public TokenService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public (RefreshRequestDto, RefreshToken) GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
        var username = user.UserName ?? string.Empty;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            }),

            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // We should save refresh token into DB
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserName = username,
            Expires = DateTime.UtcNow.AddDays(7) // refresh valide 7 jours
        };

        return (new RefreshRequestDto()
        {
            AccessToken = tokenHandler.WriteToken(token),
            AccessTokenExpiry = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
            RefreshToken = refreshToken.Token
        }, refreshToken);
    }

    // Méthode utilitaire pour extraire le principal d’un token
    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
            ValidateLifetime = true // ignore expiration
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }


    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token invalid: {ex.Message}");
            return false;
        }
    }


}

