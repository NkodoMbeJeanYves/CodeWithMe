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
[Route("v1/evaluations")]
public class EvaluationsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public EvaluationsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduEvaluations.AsNoTracking();
        if (Guid.TryParse(Request.Query["matiereId"].FirstOrDefault(), out var mid))
            query = query.Where(e => e.MatiereId == mid);
        if (Guid.TryParse(Request.Query["periodeId"].FirstOrDefault(), out var pid))
            query = query.Where(e => e.PeriodeId == pid);
        if (Guid.TryParse(Request.Query["enseignantId"].FirstOrDefault(), out var eid))
            query = query.Where(e => e.EnseignantId == eid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(e => e.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, q => e => e.Intitule.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEvaluations.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Evaluation", id.ToString());
        return ApiResults.Ok(e);
    }

    [HttpPost]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] Evaluation dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduEvaluations.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/evaluations/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Evaluation dto, CancellationToken ct)
    {
        var e = await _ctx.EduEvaluations.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Evaluation", id.ToString());
        if (dto.Intitule is not null) e.Intitule = dto.Intitule;
        if (dto.Statut is not null) e.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(e);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEvaluations.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Evaluation", id.ToString());
        _ctx.EduEvaluations.Remove(e);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpGet("{id}/notes")]
    public async Task<IActionResult> Notes(Guid id, CancellationToken ct)
    {
        var notes = await _ctx.EduNotes.AsNoTracking()
            .Where(n => n.EvaluationId == id).ToListAsync(ct);
        return ApiResults.Ok(notes);
    }

    [HttpPatch("{id}/valider")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Valider(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEvaluations.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Evaluation", id.ToString());
        e.Statut = "validee";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(e);
    }

    [HttpPatch("{id}/publier")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Publier(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEvaluations.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Evaluation", id.ToString());
        e.Statut = "publiee";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(e);
    }
}
