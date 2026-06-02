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
[Route("v1/niveaux")]
public class NiveauxController : ControllerBase
{
    private readonly ApiContext _ctx;
    public NiveauxController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduNiveaux.AsNoTracking();
        if (Guid.TryParse(Request.Query["filiereId"].FirstOrDefault(), out var fid))
            query = query.Where(n => n.FiliereId == fid);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(n => n.Actif == actif);
        var (items, meta) = await query.OrderBy(n => n.Ordre)
            .ToContractPageAsync(opts, q => n => n.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var n = await _ctx.EduNiveaux.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Niveau", id.ToString());
        return ApiResults.Ok(n);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] Niveau dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduNiveaux.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/niveaux/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Niveau dto, CancellationToken ct)
    {
        var n = await _ctx.EduNiveaux.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Niveau", id.ToString());
        if (dto.Libelle is not null) n.Libelle = dto.Libelle;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(n);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var n = await _ctx.EduNiveaux.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Niveau", id.ToString());
        n.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
