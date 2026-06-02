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
[Route("v1/inscriptions-edu")]
public class InscriptionsEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public InscriptionsEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduInscriptions.AsNoTracking();
        if (Guid.TryParse(Request.Query["anneeAcademiqueId"].FirstOrDefault(), out var aid))
            query = query.Where(i => i.AnneeAcademiqueId == aid);
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(i => i.EtablissementId == eid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(i => i.Statut == statut);
        if (bool.TryParse(Request.Query["listAttente"].FirstOrDefault(), out var la))
            query = query.Where(i => i.ListAttente == la);
        var (items, meta) = await query.ToContractPageAsync(
            opts, q => i => i.NumeroInscription.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var i = await _ctx.EduInscriptions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Inscription", id.ToString());
        return ApiResults.Ok(i);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] Inscription dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduInscriptions.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/inscriptions-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Inscription dto, CancellationToken ct)
    {
        var i = await _ctx.EduInscriptions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Inscription", id.ToString());
        if (dto.Commentaire is not null) i.Commentaire = dto.Commentaire;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(i);
    }

    [HttpPatch("{id}/soumettre")]
    public async Task<IActionResult> Soumettre(Guid id, CancellationToken ct)
        => await ChangeStatut(id, "soumise", ct);

    [HttpPatch("{id}/valider")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Valider(Guid id, CancellationToken ct)
        => await ChangeStatut(id, "validee", ct);

    [HttpPatch("{id}/rejeter")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Rejeter(Guid id, [FromBody] RejetRequest req, CancellationToken ct)
    {
        var i = await _ctx.EduInscriptions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Inscription", id.ToString());
        i.Statut = "rejetee";
        i.MotifRejet = req.Motif;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(i);
    }

    [HttpPatch("{id}/annuler")]
    public async Task<IActionResult> Annuler(Guid id, CancellationToken ct)
        => await ChangeStatut(id, "annulee", ct);

    [HttpPatch("{id}/affecter")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Affecter(Guid id, [FromBody] AffectationRequest req, CancellationToken ct)
    {
        var i = await _ctx.EduInscriptions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Inscription", id.ToString());
        i.ClasseId = req.ClasseId;
        i.PromotionId = req.PromotionId;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(i);
    }

    [HttpGet("{id}/historique")]
    public async Task<IActionResult> Historique(Guid id, CancellationToken ct)
    {
        var items = await _ctx.EduInscriptionHistoriques.AsNoTracking()
            .Where(h => h.InscriptionId == id).OrderByDescending(h => h.Date).ToListAsync(ct);
        return ApiResults.Ok(items);
    }

    [HttpGet("stats")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Stats(CancellationToken ct)
    {
        var query = _ctx.EduInscriptions.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(i => i.EtablissementId == eid);
        var stats = new
        {
            Total = await query.CountAsync(ct),
            Validees = await query.CountAsync(i => i.Statut == "validee", ct),
            EnAttente = await query.CountAsync(i => i.ListAttente, ct)
        };
        return ApiResults.Ok(stats);
    }

    private async Task<IActionResult> ChangeStatut(Guid id, string statut, CancellationToken ct)
    {
        var i = await _ctx.EduInscriptions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Inscription", id.ToString());
        i.Statut = statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(i);
    }

    public record RejetRequest(string Motif);
    public record AffectationRequest(Guid? ClasseId, Guid? PromotionId);
}
