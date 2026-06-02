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
[Route("v1/salles")]
public class SallesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public SallesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduSalles.AsNoTracking();
        var campusId = Request.Query["campusId"].FirstOrDefault();
        if (campusId is not null && Guid.TryParse(campusId, out var cId))
            query = query.Where(s => s.CampusId == cId);
        var type = Request.Query["type"].FirstOrDefault();
        if (type is not null) query = query.Where(s => s.Type == type);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(s => s.Statut == statut);
        if (int.TryParse(Request.Query["capaciteMin"].FirstOrDefault(), out var cap))
            query = query.Where(s => s.Capacite >= cap);
        var (items, meta) = await query
            .ToContractPageAsync(opts, q => s => s.Nom.Contains(q) || s.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSalles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Salle", id.ToString());
        return ApiResults.Ok(s);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Create([FromBody] Salle dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduSalles.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/salles/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Salle dto, CancellationToken ct)
    {
        var s = await _ctx.EduSalles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Salle", id.ToString());
        if (dto.Nom is not null) s.Nom = dto.Nom;
        if (dto.Type is not null) s.Type = dto.Type;
        if (dto.Capacite > 0) s.Capacite = dto.Capacite;
        if (dto.Statut is not null) s.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var s = await _ctx.EduSalles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Salle", id.ToString());
        s.Statut = "inactive";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
