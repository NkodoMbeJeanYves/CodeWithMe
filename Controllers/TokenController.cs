using CodeWithMe.Core;
using CodeWithMe.Core.Dtos.Auth;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CodeWithMe.Controllers;

[Route("api/tokens")]
[ApiController]
[Authorize]
public class TokenController : ControllerBase
{
    private readonly JwtConfig _jwtConfig;
    private readonly TokenService _tokenService;
    private readonly ApiContext _context;
    private readonly UserManager<User> _userManager;
    private string _token;

    public TokenController(IOptions<JwtConfig> jwtConfig, TokenService tokenService, ApiContext context, UserManager<User> userManager)
    {
        _jwtConfig = jwtConfig.Value;
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;

        //var authHeader = Request.Headers["Authorization"].ToString();
        //_token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

    }

    // POST /api/tokens/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.UserName == dto.Username);
        if (user == null)
            return Unauthorized();

        if (!_userManager.CheckPasswordAsync(user, dto.Password).Result)
            return Unauthorized();
        //"email": "nkodomjy@gmail.com",
        //"password": "Password@2026"

        var (tokenDto, refreshToken) = _tokenService.GenerateJwtToken(user);

        // Save refresh token to the database
        _context.RefreshTokens.Add(refreshToken);
        _context.SaveChanges();

        return Ok(new { tokenDto.AccessToken, tokenDto.AccessTokenExpiry, tokenDto.RefreshToken });
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

        // Check if the refresh token exists in the database
        var rToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && !rt.IsRevoked && rt.Expires > DateTime.UtcNow);

        if (rToken == null)
            return BadRequest("Invalid refresh token");

        // Optionally, you can also check if the username in the refresh token matches the username in the access token for added security
        //var user = HttpContext.User;
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var usernameFromAccessToken = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (rToken.UserName != usernameFromAccessToken)
            return BadRequest("Refresh token does not match the access token");

        // generate new access token
        var (accessToken, refreshToken) = _tokenService.GenerateJwtToken(new User { UserName = usernameFromAccessToken, Email = email });

        // revoke the old refresh token
        rToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return Ok(new { accessToken, refreshToken });
    }

    // Revoke a token by its JTI (JWT ID). This is useful for blacklisting tokens before they expire.
    // use NameIdentifier to get the JTI from the token and then call this endpoint to revoke it.
    [HttpPost("revoke")]
    public IActionResult Revoke([FromBody] string nameIdentifier)
    {
        var check = _context.RevokedTokens.Any(rt => rt.Jti == nameIdentifier);
        if (check)
            return BadRequest(new { Message = "Token is already revoked" });

        _context.RevokedTokens.Add(new RevokedToken
        {
            Jti = nameIdentifier,
            RevokedAt = DateTime.UtcNow
        });
        _context.SaveChanges();

        return Ok(new { Message = "Token revoked successfully" });
    }




    [HttpPost("validate")]
    public IActionResult Validate()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        _token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;
        return _tokenService.ValidateToken(_token) ? Ok(new { Valid = true }) : BadRequest(new { Valid = false });
    }

    [HttpPost("refresh-token/revoke")]
    public async Task RevokeRefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.Token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    [HttpPatch("refresh-token/validate")]
    public async Task<bool> IsRefreshTokenValid([FromBody] RefreshTokenRequestDto request)
    {
        var refreshToken = await _context.RefreshTokens.FirstAsync(rt => rt.Token == request.Token);
        return refreshToken != null && !refreshToken.IsRevoked && refreshToken.Expires > DateTime.UtcNow;
    }

    [HttpGet("me")]
    public IActionResult GetUserInfo()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        _token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { Claims = claims, Token = _token });
    }

}
