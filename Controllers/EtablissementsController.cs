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
[Route("v1/etablissements")]
public class EtablissementsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public EtablissementsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.Tenants.AsNoTracking();
        var actif = Request.Query["actif"].FirstOrDefault();
        if (actif is not null && bool.TryParse(actif, out var a))
            query = a ? query.Where(t => t.Status == TenantStatus.Active)
                      : query.Where(t => t.Status != TenantStatus.Active);
        var (items, meta) = await query
            .Select(t => new { t.Id, t.Code, t.Name, t.Type, t.Status })
            .ToContractPageAsync(opts, q => t => t.Name.Contains(q) || t.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var t = await _ctx.Tenants.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Etablissement", id.ToString());
        return ApiResults.Ok(t);
    }

    [HttpGet("{id}/campus")]
    public async Task<IActionResult> GetCampus(string id, CancellationToken ct)
    {
        var campus = await _ctx.EduCampus
            .AsNoTracking()
            .Where(c => c.EtablissementId == Guid.Parse(id))
            .ToListAsync(ct);
        return ApiResults.Ok(campus);
    }

    [HttpGet("{id}/annees-academiques")]
    public async Task<IActionResult> GetAnneesAcademiques(string id, CancellationToken ct)
    {
        var annees = await _ctx.EduAnneesAcademiques
            .AsNoTracking()
            .Where(a => a.EtablissementId == Guid.Parse(id))
            .ToListAsync(ct);
        return ApiResults.Ok(annees);
    }

    [HttpGet("{id}/calendrier")]
    public async Task<IActionResult> GetCalendrier(string id, CancellationToken ct)
    {
        var events = await _ctx.EduEvenementsCalendrier
            .AsNoTracking()
            .Where(e => e.EtablissementId == Guid.Parse(id))
            .ToListAsync(ct);
        return ApiResults.Ok(events);
    }
}
