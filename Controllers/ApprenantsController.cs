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
[Route("v1/apprenants")]
public class ApprenantsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public ApprenantsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire, Role.Enseignant)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduApprenants.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(a => a.EtablissementId == eid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(a => a.Statut == statut);
        var type = Request.Query["type"].FirstOrDefault();
        if (type is not null) query = query.Where(a => a.Type == type);
        var (items, meta) = await query.ToContractPageAsync(
            opts, q => a => a.Nom.Contains(q) || a.Prenom.Contains(q) || a.NumeroInscription.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduApprenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Apprenant", id.ToString());
        return ApiResults.Ok(a);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Apprenant dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduApprenants.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/apprenants/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Apprenant dto, CancellationToken ct)
    {
        var a = await _ctx.EduApprenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Apprenant", id.ToString());
        if (dto.Telephone is not null) a.Telephone = dto.Telephone;
        if (dto.Email is not null) a.Email = dto.Email;
        if (dto.Adresse is not null) a.Adresse = dto.Adresse;
        if (dto.Statut is not null) a.Statut = dto.Statut;
        if (dto.PhotoUrl is not null) a.PhotoUrl = dto.PhotoUrl;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(a);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduApprenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Apprenant", id.ToString());
        a.Statut = "archive";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpGet("{id}/inscriptions")]
    public async Task<IActionResult> GetInscriptions(Guid id, CancellationToken ct)
    {
        var items = await _ctx.EduInscriptions.AsNoTracking()
            .Where(i => i.ApprenantId == id)
            .ToListAsync(ct);
        return ApiResults.Ok(items);
    }

    [HttpGet("{id}/notes")]
    public async Task<IActionResult> GetNotes(Guid id, CancellationToken ct)
    {
        var query = _ctx.EduNotes.AsNoTracking().Where(n => n.ApprenantId == id);
        if (Guid.TryParse(Request.Query["periodeId"].FirstOrDefault(), out var pid))
            query = query.Include(n => n.Evaluation).Where(n => n.Evaluation.PeriodeId == pid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpGet("{id}/moyennes")]
    public async Task<IActionResult> GetMoyennes(Guid id, CancellationToken ct)
    {
        var query = _ctx.EduMoyennesGenerales.AsNoTracking().Where(m => m.ApprenantId == id);
        if (Guid.TryParse(Request.Query["anneeAcademiqueId"].FirstOrDefault(), out var aid))
            query = query.Where(m => m.AnneeAcademiqueId == aid);
        return ApiResults.Ok(await query.ToListAsync(ct));
    }

    [HttpPost("{id}/tuteurs")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> AddTuteur(Guid id, [FromBody] Tuteur dto, CancellationToken ct)
    {
        var a = await _ctx.EduApprenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Apprenant", id.ToString());
        dto.Id = Guid.NewGuid();
        dto.ApprenantId = id;
        _ctx.EduTuteurs.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/apprenants/{id}/tuteurs/{dto.Id}", dto);
    }

    [HttpPatch("{id}/tuteurs/{tuteurId}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> UpdateTuteur(Guid id, Guid tuteurId, [FromBody] Tuteur dto, CancellationToken ct)
    {
        var t = await _ctx.EduTuteurs.FirstOrDefaultAsync(t => t.Id == tuteurId && t.ApprenantId == id, ct)
                ?? throw NotFoundException.For("Tuteur", tuteurId.ToString());
        if (dto.Telephone is not null) t.Telephone = dto.Telephone;
        if (dto.Email is not null) t.Email = dto.Email;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(t);
    }

    [HttpDelete("{id}/tuteurs/{tuteurId}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> DeleteTuteur(Guid id, Guid tuteurId, CancellationToken ct)
    {
        var t = await _ctx.EduTuteurs.FirstOrDefaultAsync(t => t.Id == tuteurId && t.ApprenantId == id, ct)
                ?? throw NotFoundException.For("Tuteur", tuteurId.ToString());
        _ctx.EduTuteurs.Remove(t);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpGet("stats")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Stats(CancellationToken ct)
    {
        var query = _ctx.EduApprenants.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(a => a.EtablissementId == eid);
        var stats = new
        {
            Total = await query.CountAsync(ct),
            Actifs = await query.CountAsync(a => a.Statut == "actif", ct),
            Inactifs = await query.CountAsync(a => a.Statut != "actif", ct)
        };
        return ApiResults.Ok(stats);
    }
}
