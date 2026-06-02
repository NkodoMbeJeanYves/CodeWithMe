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
[Route("v1/absences-edu")]
public class AbsencesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public AbsencesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduAbsences.AsNoTracking();
        if (Guid.TryParse(Request.Query["apprenantId"].FirstOrDefault(), out var aid))
            query = query.Where(a => a.ApprenantId == aid);
        if (Guid.TryParse(Request.Query["classeId"].FirstOrDefault(), out var cid))
            query = query.Where(a => a.ClasseId == cid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(a => a.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduAbsences.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Absence", id.ToString());
        return ApiResults.Ok(a);
    }

    [HttpPost("{id}/justificatif")]
    public async Task<IActionResult> SoumettreJustificatif(Guid id, [FromBody] Justificatif dto, CancellationToken ct)
    {
        var a = await _ctx.EduAbsences.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Absence", id.ToString());
        dto.Id = Guid.NewGuid();
        dto.AbsenceId = id;
        _ctx.EduJustificatifs.Add(dto);
        a.Statut = "en_attente_justification";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/absences-edu/{id}/justificatif", dto);
    }

    [HttpGet("stats-apprenant/{apprenantId}")]
    public async Task<IActionResult> StatsApprenant(Guid apprenantId, CancellationToken ct)
    {
        var query = _ctx.EduAbsences.AsNoTracking().Where(a => a.ApprenantId == apprenantId);
        var stats = new
        {
            Total = await query.CountAsync(ct),
            Justifiees = await query.CountAsync(a => a.Statut == "justifiee", ct),
            NonJustifiees = await query.CountAsync(a => a.Statut == "non_justifiee", ct),
            TotalHeures = await query.SumAsync(a => a.DureeHeures, ct)
        };
        return ApiResults.Ok(stats);
    }
}
