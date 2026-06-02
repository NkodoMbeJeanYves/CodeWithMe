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
[Route("v1/creneaux")]
public class CreneauxController : ControllerBase
{
    private readonly ApiContext _ctx;
    public CreneauxController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduCreneauxHoraires.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(c => c.EtablissementId == eid);
        return ApiResults.Ok(await query.OrderBy(c => c.Ordre).ToListAsync(ct));
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] CreneauHoraire dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduCreneauxHoraires.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/creneaux/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreneauHoraire dto, CancellationToken ct)
    {
        var c = await _ctx.EduCreneauxHoraires.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("CreneauHoraire", id.ToString());
        if (dto.Libelle is not null) c.Libelle = dto.Libelle;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(c);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduCreneauxHoraires.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("CreneauHoraire", id.ToString());
        c.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
