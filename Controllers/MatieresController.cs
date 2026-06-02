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
[Route("v1/matieres")]
public class MatieresController : ControllerBase
{
    private readonly ApiContext _ctx;
    public MatieresController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduMatieres.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(m => m.EtablissementId == eid);
        if (Guid.TryParse(Request.Query["filiereId"].FirstOrDefault(), out var fid))
            query = query.Where(m => m.FiliereId == fid);
        if (Guid.TryParse(Request.Query["niveauId"].FirstOrDefault(), out var nid))
            query = query.Where(m => m.NiveauId == nid);
        if (Guid.TryParse(Request.Query["ueId"].FirstOrDefault(), out var uid))
            query = query.Where(m => m.UeId == uid);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(m => m.Actif == actif);
        var (items, meta) = await query.ToContractPageAsync(opts, q => m => m.Libelle.Contains(q) || m.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var m = await _ctx.EduMatieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Matiere", id.ToString());
        return ApiResults.Ok(m);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] Matiere dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduMatieres.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/matieres/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Matiere dto, CancellationToken ct)
    {
        var m = await _ctx.EduMatieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Matiere", id.ToString());
        if (dto.Libelle is not null) m.Libelle = dto.Libelle;
        if (dto.Coefficient > 0) m.Coefficient = dto.Coefficient;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(m);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var m = await _ctx.EduMatieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Matiere", id.ToString());
        m.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpPost("{id}/rattacher-ue")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> RattacherUE(Guid id, [FromBody] RattacherUERequest req, CancellationToken ct)
    {
        var m = await _ctx.EduMatieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Matiere", id.ToString());
        m.UeId = req.UeId;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(m);
    }

    public record RattacherUERequest(Guid UeId);
}
