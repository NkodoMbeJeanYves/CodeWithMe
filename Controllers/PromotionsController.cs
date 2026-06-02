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
[Route("v1/promotions")]
public class PromotionsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public PromotionsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduPromotions.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(p => p.EtablissementId == eid);
        if (Guid.TryParse(Request.Query["anneeAcademiqueId"].FirstOrDefault(), out var aid))
            query = query.Where(p => p.AnneeAcademiqueId == aid);
        var (items, meta) = await query.ToContractPageAsync(opts, q => p => p.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPromotions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Promotion", id.ToString());
        return ApiResults.Ok(p);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] Promotion dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduPromotions.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/promotions/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Promotion dto, CancellationToken ct)
    {
        var p = await _ctx.EduPromotions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Promotion", id.ToString());
        if (dto.Libelle is not null) p.Libelle = dto.Libelle;
        if (dto.Statut is not null) p.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(p);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var p = await _ctx.EduPromotions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Promotion", id.ToString());
        p.Statut = "inactive";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
