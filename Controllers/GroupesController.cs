using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/groupes")]
public class GroupesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public GroupesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduGroupes.AsNoTracking();
        if (Guid.TryParse(Request.Query["promotionId"].FirstOrDefault(), out var pid))
            query = query.Where(g => g.PromotionId == pid);
        var type = Request.Query["type"].FirstOrDefault();
        if (type is not null) query = query.Where(g => g.Type == type);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(g => g.Actif == actif);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var g = await _ctx.EduGroupes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Groupe", id.ToString());
        return ApiResults.Ok(g);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Groupe dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduGroupes.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/groupes/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Groupe dto, CancellationToken ct)
    {
        var g = await _ctx.EduGroupes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Groupe", id.ToString());
        if (dto.Libelle is not null) g.Libelle = dto.Libelle;
        if (dto.CapaciteMax > 0) g.CapaciteMax = dto.CapaciteMax;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(g);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var g = await _ctx.EduGroupes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Groupe", id.ToString());
        g.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
