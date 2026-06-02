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
[Route("v1/epreuves")]
public class EpreuvesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public EpreuvesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduEpreuves.AsNoTracking();
        if (Guid.TryParse(Request.Query["sessionId"].FirstOrDefault(), out var sid))
            query = query.Where(e => e.SessionId == sid);
        if (Guid.TryParse(Request.Query["matiereId"].FirstOrDefault(), out var mid))
            query = query.Where(e => e.MatiereId == mid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEpreuves.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Epreuve", id.ToString());
        return ApiResults.Ok(e);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Epreuve dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduEpreuves.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/epreuves/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Epreuve dto, CancellationToken ct)
    {
        var e = await _ctx.EduEpreuves.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Epreuve", id.ToString());
        if (dto.DureeMinutes > 0) e.DureeMinutes = dto.DureeMinutes;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(e);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEpreuves.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Epreuve", id.ToString());
        _ctx.EduEpreuves.Remove(e);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpPost("{id}/convocations/generer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> GenererConvocations(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEpreuves.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Epreuve", id.ToString());
        e.ConvocationsGenerees = true;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(new { Message = "Convocations générées", EpreuveId = id });
    }
}
