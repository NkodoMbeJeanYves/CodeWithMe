using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Auth;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

/// <summary>
/// Endpoints d'authentification conformes au contrat EDU Platform v1 section 7.1 :
/// /v1/auth/login, /v1/auth/refresh, /v1/auth/logout, /v1/auth/switch-tenant, /v1/auth/me, /v1/me/tenants.
/// Wrapper au-dessus d'ASP.NET Identity (MapIdentityApi est conservé pour la gestion de mot de passe natif).
/// </summary>
[ApiController]
[Route("v1")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly ApiContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<User> userManager,
        TokenService tokenService,
        ApiContext context,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
        _logger = logger;
    }

    [HttpPost("auth/login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestV1Dto dto, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthenticatedException("Invalid email or password.");

        var (accessToken, expiry, refreshToken) = _tokenService.GenerateContractJwt(user);

        _context.RefreshTokens.Add(refreshToken);
        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        var seconds = (int)Math.Max(1, (expiry - DateTime.UtcNow).TotalSeconds);
        var response = new LoginResponseV1Dto(
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token,
            ExpiresIn: seconds,
            User: ToAuthenticatedUser(user)
        );
        return ApiResults.Ok(response);
    }

    [HttpPost("auth/refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequestV1Dto dto, CancellationToken ct)
    {
        var stored = await _context.RefreshTokens.FirstOrDefaultAsync(
            rt => rt.Token == dto.RefreshToken && !rt.IsRevoked && rt.Expires > DateTime.UtcNow, ct);
        if (stored is null)
            throw new UnauthenticatedException("Invalid or expired refresh token.", code: "TOKEN_EXPIRED");

        var user = await _userManager.FindByNameAsync(stored.UserName);
        if (user is null)
            throw new UnauthenticatedException();

        // Rotation : on révoque l'ancien refresh token et on en émet un nouveau.
        stored.IsRevoked = true;

        var (accessToken, expiry, newRefresh) = _tokenService.GenerateContractJwt(user);
        _context.RefreshTokens.Add(newRefresh);
        await _context.SaveChangesAsync(ct);

        var seconds = (int)Math.Max(1, (expiry - DateTime.UtcNow).TotalSeconds);
        return ApiResults.Ok(new LoginResponseV1Dto(accessToken, newRefresh.Token, seconds, ToAuthenticatedUser(user)));
    }

    [HttpPost("auth/logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto? dto, CancellationToken ct)
    {
        var jti = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(jti) && !await _context.RevokedTokens.AnyAsync(rt => rt.Jti == jti, ct))
        {
            _context.RevokedTokens.Add(new RevokedToken
            {
                Jti = jti,
                RevokedAt = DateTime.UtcNow
            });
        }

        if (dto is not null && !string.IsNullOrEmpty(dto.Token))
        {
            var rt = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == dto.Token, ct);
            if (rt is not null) rt.IsRevoked = true;
        }

        await _context.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    /// <summary>
    /// Réémet un JWT pour un autre tenant accessible. V1 : réservé super_admin.
    /// </summary>
    [HttpPost("auth/switch-tenant")]
    [Authorize]
    public async Task<IActionResult> SwitchTenant([FromBody] SwitchTenantRequestDto dto, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User)
                   ?? throw new UnauthenticatedException();

        if (user.Role != Role.SuperAdmin)
            throw new ForbiddenException("Only super_admin can switch tenants in V1.");

        var tenant = await _context.Tenants.FindAsync(new object[] { dto.TenantId }, ct)
                     ?? throw NotFoundException.For("Tenant", dto.TenantId);

        // On réémet un JWT en remplaçant TenantId par celui demandé.
        var originalTenant = user.TenantId;
        user.TenantId = tenant.Id;
        var (accessToken, expiry, newRefresh) = _tokenService.GenerateContractJwt(user);
        user.TenantId = originalTenant; // ne pas persister le changement, juste émettre

        _context.RefreshTokens.Add(newRefresh);
        await _context.SaveChangesAsync(ct);

        var seconds = (int)Math.Max(1, (expiry - DateTime.UtcNow).TotalSeconds);
        return ApiResults.Ok(new LoginResponseV1Dto(accessToken, newRefresh.Token, seconds, ToAuthenticatedUser(user)));
    }

    [HttpGet("auth/me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User)
                   ?? throw new UnauthenticatedException();

        var permissions = new List<string>
        {
            $"role:{System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(user.Role.ToString())}",
            $"authority:{System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(AuthorityMap.For(user.Role).ToString())}"
        };
        if (!string.IsNullOrEmpty(user.TenantId))
            permissions.Add($"tenant:{user.TenantId}");

        return ApiResults.Ok(new MeResponseV1Dto(ToAuthenticatedUser(user), permissions));
    }

    /// <summary>
    /// Tenants accessibles à l'utilisateur courant.
    /// V1 : super_admin → tous ; autres rôles → leur seul tenant rattaché.
    /// </summary>
    [HttpGet("me/tenants")]
    [Authorize]
    public async Task<IActionResult> MyTenants(CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(User)
                   ?? throw new UnauthenticatedException();

        // Le filtre global tenant peut bloquer la lecture de tenants[] pour non-super_admin —
        // on contourne via IgnoreQueryFilters quand nécessaire (Tenant n'est pas ITenantScoped en réalité,
        // donc pas filtré, mais on garde la précaution).
        IReadOnlyList<Tenant> tenants;
        if (user.Role == Role.SuperAdmin)
        {
            tenants = await _context.Tenants.AsNoTracking().ToListAsync(ct);
        }
        else if (!string.IsNullOrEmpty(user.TenantId))
        {
            tenants = await _context.Tenants
                .AsNoTracking()
                .Where(t => t.Id == user.TenantId)
                .ToListAsync(ct);
        }
        else
        {
            tenants = Array.Empty<Tenant>();
        }

        var data = tenants.Select(t => new
        {
            id = t.Id,
            code = t.Code,
            name = t.Name,
            type = t.Type,
            status = t.Status
        }).ToList();

        return ApiResults.Ok(data);
    }

    private static AuthenticatedUserDto ToAuthenticatedUser(User u) => new(
        Id: u.Id,
        TenantId: u.TenantId,
        Email: u.Email ?? string.Empty,
        FirstName: u.FirstName,
        LastName: u.LastName,
        Role: u.Role,
        AuthorityLevel: AuthorityMap.For(u.Role),
        Status: "active"
    );
}
