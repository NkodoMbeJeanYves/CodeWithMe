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
[Route("v1/seances-edu")]
public class SeancesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public SeancesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduSeances.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(s => s.EtablissementId == eid);
        if (Guid.TryParse(Request.Query["enseignantId"].FirstOrDefault(), out var enid))
            query = query.Where(s => s.EnseignantId == enid);
        if (Guid.TryParse(Request.Query["classeId"].FirstOrDefault(), out var cid))
            query = query.Where(s => s.ClasseId == cid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(s => s.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        return ApiResults.Ok(s);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Seance dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduSeances.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/seances-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Seance dto, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        if (dto.Statut is not null) s.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        _ctx.EduSeances.Remove(s);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpPatch("{id}/annuler")]
    public async Task<IActionResult> Annuler(Guid id, [FromBody] MotifRequest req, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        s.Statut = "annulee";
        s.MotifAnnulation = req.Motif;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpPatch("{id}/realiser")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Realiser(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        s.Statut = "realisee";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpPatch("{id}/cahier-texte")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> CahierTexte(Guid id, [FromBody] CahierTexteRequest req, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        s.ContenuEnseignant = req.Contenu;
        s.TravauxDemandes = req.Travaux;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpPatch("{id}/remplacer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Remplacer(Guid id, [FromBody] RemplacerRequest req, CancellationToken ct)
    {
        var s = await _ctx.EduSeances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Seance", id.ToString());
        s.EnseignantRemplacantId = req.EnseignantRemplacantId;
        s.EstRemplacement = true;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    public record MotifRequest(string Motif);
    public record CahierTexteRequest(string? Contenu, string? Travaux);
    public record RemplacerRequest(Guid EnseignantRemplacantId);
}
