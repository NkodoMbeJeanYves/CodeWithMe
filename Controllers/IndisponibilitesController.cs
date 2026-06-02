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
[Route("v1/indisponibilites")]
public class IndisponibilitesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public IndisponibilitesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduIndisponibilites.AsNoTracking();
        if (Guid.TryParse(Request.Query["enseignantId"].FirstOrDefault(), out var eid))
            query = query.Where(i => i.EnseignantId == eid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpPost]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Indisponibilite dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduIndisponibilites.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/indisponibilites/{dto.Id}", dto);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var i = await _ctx.EduIndisponibilites.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Indisponibilite", id.ToString());
        _ctx.EduIndisponibilites.Remove(i);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
