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
[Route("v1/cycles")]
public class CyclesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public CyclesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduCycles.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(c => c.EtablissementId == eid);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(c => c.Actif == actif);
        var (items, meta) = await query.ToContractPageAsync(opts, q => c => c.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduCycles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Cycle", id.ToString());
        return ApiResults.Ok(c);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] Cycle dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduCycles.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/cycles/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Cycle dto, CancellationToken ct)
    {
        var c = await _ctx.EduCycles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Cycle", id.ToString());
        if (dto.Libelle is not null) c.Libelle = dto.Libelle;
        if (dto.Description is not null) c.Description = dto.Description;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(c);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduCycles.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Cycle", id.ToString());
        c.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
