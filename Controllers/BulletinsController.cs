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
[Route("v1/bulletins")]
public class BulletinsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public BulletinsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduBulletins.AsNoTracking();
        if (Guid.TryParse(Request.Query["apprenantId"].FirstOrDefault(), out var aid))
            query = query.Where(b => b.ApprenantId == aid);
        if (Guid.TryParse(Request.Query["periodeId"].FirstOrDefault(), out var pid))
            query = query.Where(b => b.PeriodeId == pid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(b => b.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, null, ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var b = await _ctx.EduBulletins.Include(b => b.Lignes)
            .FirstOrDefaultAsync(b => b.Id == id, ct)
            ?? throw NotFoundException.For("Bulletin", id.ToString());
        return ApiResults.Ok(b);
    }

    [HttpPost("generer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Generer([FromBody] GenererRequest req, CancellationToken ct)
    {
        // Placeholder — la logique réelle necessite le calcul des moyennes
        return ApiResults.Ok(new { Message = "Génération initiée", ClasseId = req.ClasseId, PeriodeId = req.PeriodeId });
    }

    [HttpPatch("{id}/valider")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Valider(Guid id, CancellationToken ct)
    {
        var b = await _ctx.EduBulletins.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Bulletin", id.ToString());
        b.Statut = "valide";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(b);
    }

    [HttpPatch("{id}/signer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Signer(Guid id, [FromBody] SignerRequest req, CancellationToken ct)
    {
        var b = await _ctx.EduBulletins.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Bulletin", id.ToString());
        b.SignePar = req.SignePar;
        b.DateSigne = DateTime.UtcNow;
        b.Statut = "signe";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(b);
    }

    [HttpPatch("{id}/publier")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Publier(Guid id, CancellationToken ct)
    {
        var b = await _ctx.EduBulletins.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Bulletin", id.ToString());
        b.Statut = "publie";
        b.DatePublication = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(b);
    }

    public record GenererRequest(Guid ClasseId, Guid PeriodeId);
    public record SignerRequest(string SignePar);
}
