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
[Route("v1/enseignants-edu")]
public class EnseignantsEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public EnseignantsEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire, Role.DirecteurPedagogique)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduEnseignants.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(e => e.EtablissementId == eid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(e => e.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(
            opts, q => e => e.Nom.Contains(q) || e.Prenom.Contains(q) || e.Matricule.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEnseignants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Enseignant", id.ToString());
        return ApiResults.Ok(e);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Create([FromBody] Enseignant dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduEnseignants.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/enseignants-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Enseignant dto, CancellationToken ct)
    {
        var e = await _ctx.EduEnseignants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Enseignant", id.ToString());
        if (dto.Telephone is not null) e.Telephone = dto.Telephone;
        if (dto.Email is not null) e.Email = dto.Email;
        if (dto.Statut is not null) e.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(e);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEnseignants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Enseignant", id.ToString());
        e.Statut = "inactif";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpPost("{id}/affecter-matiere")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> AffecterMatiere(Guid id, [FromBody] AffectationMatiere dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        dto.EnseignantId = id;
        _ctx.EduAffectationsMatieres.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/enseignants-edu/{id}/affectations/{dto.Id}", dto);
    }

    [HttpPatch("{id}/affectations/{affId}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> UpdateAffectation(Guid id, Guid affId, [FromBody] AffectationMatiere dto, CancellationToken ct)
    {
        var aff = await _ctx.EduAffectationsMatieres
            .FirstOrDefaultAsync(a => a.Id == affId && a.EnseignantId == id, ct)
            ?? throw NotFoundException.For("AffectationMatiere", affId.ToString());
        if (dto.HeuresPrevues > 0) aff.HeuresPrevues = dto.HeuresPrevues;
        aff.HeuresRealisees = dto.HeuresRealisees;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(aff);
    }

    [HttpDelete("{id}/affectations/{affId}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> DeleteAffectation(Guid id, Guid affId, CancellationToken ct)
    {
        var aff = await _ctx.EduAffectationsMatieres
            .FirstOrDefaultAsync(a => a.Id == affId && a.EnseignantId == id, ct)
            ?? throw NotFoundException.For("AffectationMatiere", affId.ToString());
        aff.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpGet("{id}/charge-horaire")]
    public async Task<IActionResult> ChargeHoraire(Guid id, CancellationToken ct)
    {
        var affectations = await _ctx.EduAffectationsMatieres.AsNoTracking()
            .Where(a => a.EnseignantId == id && a.Actif)
            .ToListAsync(ct);
        var stats = new
        {
            HeuresPrevuesTotales = affectations.Sum(a => a.HeuresPrevues),
            HeuresRealisees = affectations.Sum(a => a.HeuresRealisees),
            NombreAffectations = affectations.Count
        };
        return ApiResults.Ok(stats);
    }

    [HttpGet("stats")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Stats(CancellationToken ct)
    {
        var query = _ctx.EduEnseignants.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(e => e.EtablissementId == eid);
        var stats = new
        {
            Total = await query.CountAsync(ct),
            Actifs = await query.CountAsync(e => e.Statut == "actif", ct)
        };
        return ApiResults.Ok(stats);
    }
}
