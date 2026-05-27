using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Admin;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _users;
    private readonly ApiContext _ctx;
    public UsersController(UserManager<User> users, ApiContext ctx) { _users = users; _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Directeur, Role.ResponsableAdministratif, Role.SuperAdmin)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _users.Users.AsNoTracking()
            .Select(u => new UserAdminDto(
                u.Id, u.Email ?? "", u.FirstName, u.LastName, u.Role, u.TenantId,
                u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow ? "suspended" : "active",
                u.LastLoginAt))
            .ToContractPageAsync(opts, q => u => u.Email!.Contains(q) || u.FirstName!.Contains(q) || u.LastName!.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto, CancellationToken ct)
    {
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            TenantId = dto.TenantId,
            EmailConfirmed = true
        };
        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(
                "User creation failed: " + string.Join("; ", result.Errors.Select(e => e.Description)),
                details: result.Errors.Select(e => e.Code).ToArray());
        }

        return ApiResults.Created($"/v1/users/{user.Id}", new UserAdminDto(
            user.Id, user.Email ?? "", user.FirstName, user.LastName, user.Role, user.TenantId, "active", null));
    }

    [HttpPatch("{id}/role")]
    [AllowRoles(Role.Directeur, Role.SuperAdmin)]
    public async Task<IActionResult> ChangeRole(string id, [FromBody] UserRoleUpdateDto dto, CancellationToken ct)
    {
        var user = await _users.FindByIdAsync(id) ?? throw NotFoundException.For("User", id);
        user.Role = dto.Role;
        await _users.UpdateAsync(user);
        return ApiResults.Ok(new UserAdminDto(
            user.Id, user.Email ?? "", user.FirstName, user.LastName, user.Role, user.TenantId, "active", user.LastLoginAt));
    }

    [HttpPost("{id}/suspend")]
    [AllowRoles(Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Suspend(string id, CancellationToken ct)
    {
        var user = await _users.FindByIdAsync(id) ?? throw NotFoundException.For("User", id);
        await _users.SetLockoutEnabledAsync(user, true);
        await _users.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        return ApiResults.NoContent();
    }
}
