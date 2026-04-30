using CodeWithMe.Core;
using CodeWithMe.Core.Dtos.Auth;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CodeWithMe.Controllers;

[Route("api/tokens")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly JwtConfig _jwtConfig;
    private readonly TokenService _tokenService;
    private readonly ApiContext _context;
    private readonly UserManager<User> _userManager;

    public TokenController(IOptions<JwtConfig> jwtConfig, TokenService tokenService, ApiContext context, UserManager<User> userManager)
    {
        _jwtConfig = jwtConfig.Value;
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
    }

    // POST /api/tokens/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.UserName == dto.Username);
        if (user == null)
            return Unauthorized();

        if (!_userManager.CheckPasswordAsync(user, dto.Password).Result)
            return Unauthorized();
        //"email": "nkodomjy@gmail.com",
        //"password": "Password@2026"

        var (tokenDto, refreshToken) = _tokenService.GenerateJwtToken(dto.Username);

        // Save refresh token to the database
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return Ok(new { token = tokenDto.AccessToken, Expires = tokenDto.AccessTokenExpiry });
    }

    /**
     * Client sends the expired access token and the refresh token to this endpoint. 
     * The server validates the refresh token and, if valid, issues a new access token.
     * 
     * Checking refresh Token : Exist in DB, Not Revoked, Not Expired, 
     * and check if the username in the refresh token matches the username in the access token (optional but recommended for security).
     * 
     * If everything is valid, generate a new access token and optionally a new refresh token, and return them to the client.
     */
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
    {
        if (_tokenService.ValidateToken(dto.AccessToken))
            return Ok("Token Is Still Valid");

        var principal = _tokenService.GetPrincipalFromToken(dto.AccessToken);
        if (principal == null)
            return BadRequest("Invalid access token");

        // Check if the refresh token exists in the database
        var rToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && !rt.IsRevoked && rt.Expires > DateTime.UtcNow);

        if (rToken == null)
            return BadRequest("Invalid refresh token");

        // Optionally, you can also check if the username in the refresh token matches the username in the access token for added security
        var usernameFromAccessToken = principal.Identity?.Name;
        if (rToken.UserName != usernameFromAccessToken)
            return BadRequest("Refresh token does not match the access token");

        // generate new access token
        var (accessToken, refreshToken) = _tokenService.GenerateJwtToken(principal?.Identity?.Name ?? "UserName");

        // revoke the old refresh token
        rToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return Ok(new { accessToken, refreshToken });
    }



    [HttpPost("/{token}/validate")]
    [Authorize]
    public IActionResult Validate([FromRoute] string token)
    {
        return _tokenService.ValidateToken(token) ? Ok(new { Valid = true }) : BadRequest(new { Valid = false });
    }

    [HttpPost("refresh/{token}/revoke")]
    public async Task RevokeRefreshToken(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    [HttpPatch("refresh/{token}/check")]
    public async Task<bool> IsRefreshTokenValid(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstAsync(rt => rt.Token == token);
        return refreshToken != null && !refreshToken.IsRevoked && refreshToken.Expires > DateTime.UtcNow;
    }

}
