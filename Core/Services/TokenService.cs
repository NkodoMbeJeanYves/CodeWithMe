using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeWithMe.Core.Dtos.Auth;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.IdentityModel.Tokens;

namespace CodeWithMe.Core.Services;

public class TokenService
{
    private readonly JwtConfig _jwtConfig;

    public TokenService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    /// <summary>
    /// Émission JWT historique (compat <see cref="Controllers.TokenController"/>).
    /// </summary>
    public (RefreshRequestDto, RefreshToken) GenerateJwtToken(User user)
    {
        var (access, exp, refresh) = BuildToken(user, includeContractClaims: false);
        return (new RefreshRequestDto
        {
            AccessToken = access,
            AccessTokenExpiry = exp,
            RefreshToken = refresh.Token
        }, refresh);
    }

    /// <summary>
    /// Émission JWT enrichi (contrat EDU Platform v1 section 7.2 : tenantId, role, authorityLevel, ...).
    /// Utilisé par <see cref="Controllers.AuthController"/>.
    /// </summary>
    public (string AccessToken, DateTime Expiry, RefreshToken Refresh) GenerateContractJwt(User user)
    {
        return BuildToken(user, includeContractClaims: true);
    }

    private (string AccessToken, DateTime Expiry, RefreshToken Refresh) BuildToken(User user, bool includeContractClaims)
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);
        var username = user.UserName ?? user.Email ?? string.Empty;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), // JTI utilisé par JwtRevocationMiddleware
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        if (includeContractClaims)
        {
            if (!string.IsNullOrEmpty(user.TenantId))
                claims.Add(new Claim("tenantId", user.TenantId));

            var roleString = ToSnake(user.Role.ToString());
            claims.Add(new Claim("role", roleString));
            claims.Add(new Claim(ClaimTypes.Role, roleString));
            claims.Add(new Claim("authorityLevel", ToSnake(AuthorityMap.For(user.Role).ToString())));
        }

        var expiry = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiry,
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserName = username,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        return (accessToken, expiry, refreshToken);
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwt ||
            !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
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
            }, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string ToSnake(string pascal)
        => System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(pascal);
}
