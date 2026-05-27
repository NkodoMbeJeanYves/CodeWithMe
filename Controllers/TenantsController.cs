using System.Text.Json;
using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Admin;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantModel = CodeWithMe.Core.Models.Tenant;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public TenantsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Tenants.AsNoTracking()
            .Select(t => new TenantAdminDto(t.Code, t.Name, t.Type, t.Status, t.Locale, t.Timezone, t.AcademicYear, t.Id))
            .ToContractPageAsync(opts, q => t => t.Name.Contains(q) || t.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin)]
    public async Task<IActionResult> Create([FromBody] TenantCreateDto dto, CancellationToken ct)
    {
        var entity = new TenantModel
        {
            Id = Guid.NewGuid().ToString(),
            Code = dto.Code,
            Name = dto.Name,
            Type = dto.Type,
            Status = TenantStatus.Active,
            Locale = dto.Locale,
            Timezone = dto.Timezone,
            AcademicYear = dto.AcademicYear
        };
        _ctx.Tenants.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/tenants/{entity.Id}", ToDto(entity));
    }

    [HttpGet("{id}/settings")]
    [AllowRoles(Role.Directeur, Role.SuperAdmin)]
    public async Task<IActionResult> GetSettings(string id, CancellationToken ct)
    {
        var t = await _ctx.Tenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Tenant", id);
        var settings = string.IsNullOrEmpty(t.SettingsJson)
            ? new TenantSettingsDto(20, 1)
            : JsonSerializer.Deserialize<TenantSettingsDto>(t.SettingsJson) ?? new TenantSettingsDto(20, 1);
        return ApiResults.Ok(settings);
    }

    [HttpPut("{id}/settings")]
    [AllowRoles(Role.Directeur, Role.SuperAdmin)]
    public async Task<IActionResult> UpdateSettings(string id, [FromBody] TenantSettingsDto dto, CancellationToken ct)
    {
        var t = await _ctx.Tenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Tenant", id);
        t.SettingsJson = JsonSerializer.Serialize(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(dto);
    }

    private static TenantAdminDto ToDto(TenantModel t) =>
        new(t.Code, t.Name, t.Type, t.Status, t.Locale, t.Timezone, t.AcademicYear, t.Id);
}
