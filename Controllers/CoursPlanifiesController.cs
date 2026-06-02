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
[Route("v1/cours-planifies")]
public class CoursPlanifiesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public CoursPlanifiesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var query = _ctx.EduCoursPlanifies.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(cp => cp.EtablissementId == eid);
        if (Guid.TryParse(Request.Query["anneeAcademiqueId"].FirstOrDefault(), out var aid))
            query = query.Where(cp => cp.AnneeAcademiqueId == aid);
        if (Guid.TryParse(Request.Query["classeId"].FirstOrDefault(), out var cid))
            query = query.Where(cp => cp.ClasseId == cid);
        if (Guid.TryParse(Request.Query["promotionId"].FirstOrDefault(), out var pid))
            query = query.Where(cp => cp.PromotionId == pid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var cp = await _ctx.EduCoursPlanifies.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("CoursPlanifie", id.ToString());
        return ApiResults.Ok(cp);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] CoursPlanifie dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduCoursPlanifies.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/cours-planifies/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CoursPlanifie dto, CancellationToken ct)
    {
        var cp = await _ctx.EduCoursPlanifies.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("CoursPlanifie", id.ToString());
        if (dto.Statut is not null) cp.Statut = dto.Statut;
        if (dto.Note is not null) cp.Note = dto.Note;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(cp);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var cp = await _ctx.EduCoursPlanifies.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("CoursPlanifie", id.ToString());
        cp.Statut = "supprime";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpPost("{id}/generer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> GenererSeances(Guid id, CancellationToken ct)
    {
        var cp = await _ctx.EduCoursPlanifies.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("CoursPlanifie", id.ToString());
        // Generate séances based on recurrence
        var seances = new List<Seance>();
        var current = cp.DateDebutValidite;
        while (current <= cp.DateFinValidite)
        {
            if ((int)current.DayOfWeek == cp.JourSemaine)
            {
                seances.Add(new Seance
                {
                    Id = Guid.NewGuid(),
                    CoursPlanifieId = cp.Id,
                    EtablissementId = cp.EtablissementId,
                    MatiereId = cp.MatiereId,
                    EnseignantId = cp.EnseignantId,
                    ClasseId = cp.ClasseId,
                    PromotionId = cp.PromotionId,
                    GroupeId = cp.GroupeId,
                    SalleId = cp.SalleId,
                    TypeCours = cp.TypeCours,
                    Date = current,
                    HeureDebut = cp.HeureDebut,
                    HeureFin = cp.HeureFin,
                    DureeMinutes = (int)(cp.HeureFin - cp.HeureDebut).TotalMinutes
                });
            }
            current = current.AddDays(1);
        }
        _ctx.EduSeances.AddRange(seances);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(new { Generated = seances.Count });
    }
}
