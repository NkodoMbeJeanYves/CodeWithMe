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
[Route("v1/filieres-edu")]
public class FilieresEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public FilieresEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduFilieres.AsNoTracking();
        if (Guid.TryParse(Request.Query["cycleId"].FirstOrDefault(), out var cid))
            query = query.Where(f => f.CycleId == cid);
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(f => f.EtablissementId == eid);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(f => f.Actif == actif);
        var (items, meta) = await query.ToContractPageAsync(opts, q => f => f.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var f = await _ctx.EduFilieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("FiliereEdu", id.ToString());
        return ApiResults.Ok(f);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] FiliereEdu dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduFilieres.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/filieres-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] FiliereEdu dto, CancellationToken ct)
    {
        var f = await _ctx.EduFilieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("FiliereEdu", id.ToString());
        if (dto.Libelle is not null) f.Libelle = dto.Libelle;
        if (dto.Description is not null) f.Description = dto.Description;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(f);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var f = await _ctx.EduFilieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("FiliereEdu", id.ToString());
        f.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
