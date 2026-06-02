using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/sessions-examen")]
public class SessionsExamenController : ControllerBase
{
    private readonly ApiContext _ctx;
    public SessionsExamenController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduSessionsExamen.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(s => s.EtablissementId == eid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(s => s.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, q => s => s.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSessionsExamen.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("SessionExamen", id.ToString());
        return ApiResults.Ok(s);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] SessionExamen dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduSessionsExamen.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/sessions-examen/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SessionExamen dto, CancellationToken ct)
    {
        var s = await _ctx.EduSessionsExamen.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("SessionExamen", id.ToString());
        if (dto.Libelle is not null) s.Libelle = dto.Libelle;
        if (dto.Statut is not null) s.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSessionsExamen.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("SessionExamen", id.ToString());
        _ctx.EduSessionsExamen.Remove(s);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
