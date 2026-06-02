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
[Route("v1/periodes-inscription")]
public class PeriodesInscriptionController : ControllerBase
{
    private readonly ApiContext _ctx;
    public PeriodesInscriptionController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduPeriodesInscription.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(p => p.EtablissementId == eid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] PeriodeInscription dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduPeriodesInscription.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/periodes-inscription/{dto.Id}", dto);
    }

    [HttpPatch("{id}/ouvrir")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Ouvrir(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPeriodesInscription.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("PeriodeInscription", id.ToString());
        p.Ouverte = true;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(p);
    }

    [HttpPatch("{id}/fermer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Fermer(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPeriodesInscription.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("PeriodeInscription", id.ToString());
        p.Ouverte = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(p);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPeriodesInscription.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("PeriodeInscription", id.ToString());
        _ctx.EduPeriodesInscription.Remove(p);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
