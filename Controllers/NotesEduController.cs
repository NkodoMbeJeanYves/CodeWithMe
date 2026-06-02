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
[Route("v1/notes-edu")]
public class NotesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public NotesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduNotes.AsNoTracking();
        if (Guid.TryParse(Request.Query["evaluationId"].FirstOrDefault(), out var eid))
            query = query.Where(n => n.EvaluationId == eid);
        if (Guid.TryParse(Request.Query["apprenantId"].FirstOrDefault(), out var aid))
            query = query.Where(n => n.ApprenantId == aid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(n => n.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, null, ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] NoteEdu dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduNotes.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/notes-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Update(Guid id, [FromBody] NoteEdu dto, CancellationToken ct)
    {
        var n = await _ctx.EduNotes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("NoteEdu", id.ToString());
        if (dto.Valeur.HasValue) n.Valeur = dto.Valeur;
        if (dto.Commentaire is not null) n.Commentaire = dto.Commentaire;
        if (dto.Statut is not null) n.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(n);
    }

    [HttpPost("masse")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> SaisieMasse([FromBody] List<NoteEdu> dtos, CancellationToken ct)
    {
        foreach (var dto in dtos)
        {
            var existing = await _ctx.EduNotes
                .FirstOrDefaultAsync(n => n.EvaluationId == dto.EvaluationId && n.ApprenantId == dto.ApprenantId, ct);
            if (existing is not null)
            {
                existing.Valeur = dto.Valeur;
                existing.Absent = dto.Absent;
                existing.Commentaire = dto.Commentaire;
            }
            else
            {
                dto.Id = Guid.NewGuid();
                _ctx.EduNotes.Add(dto);
            }
        }
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(new { Updated = dtos.Count });
    }
}
