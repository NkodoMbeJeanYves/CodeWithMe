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
[Route("v1/periodes-edu")]
public class PeriodesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public PeriodesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet("~/v1/annees-academiques/{anneeId}/periodes")]
    public async Task<IActionResult> ListByAnnee(Guid anneeId, CancellationToken ct)
    {
        var periodes = await _ctx.EduPeriodes.AsNoTracking()
            .Where(p => p.AnneeAcademiqueId == anneeId)
            .OrderBy(p => p.Numero)
            .ToListAsync(ct);
        return ApiResults.Ok(periodes);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] PeriodeEdu dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduPeriodes.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/periodes-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] PeriodeEdu dto, CancellationToken ct)
    {
        var p = await _ctx.EduPeriodes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("PeriodeEdu", id.ToString());
        if (dto.Libelle is not null) p.Libelle = dto.Libelle;
        if (dto.DateDebut != default) p.DateDebut = dto.DateDebut;
        if (dto.DateFin != default) p.DateFin = dto.DateFin;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(p);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPeriodes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("PeriodeEdu", id.ToString());
        _ctx.EduPeriodes.Remove(p);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
