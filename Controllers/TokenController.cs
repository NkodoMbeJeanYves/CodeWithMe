using CodeWithMe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeWithMe.Controllers;

[Route("api/tokens")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly JwtConfig _jwtConfig;
    public TokenController(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    // GET /api/token
    [HttpGet]
    public IActionResult GetToken()
    {
        var issuer = _jwtConfig.Issuer;
        var audience = _jwtConfig.Audience;
        var secretKey = _jwtConfig.SecretKey;
        var expirationMinutes = _jwtConfig.ExpirationMinutes;
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
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return Ok(new { token = accessToken, Expires = tokenExpirytimeStamp });
    }
}
