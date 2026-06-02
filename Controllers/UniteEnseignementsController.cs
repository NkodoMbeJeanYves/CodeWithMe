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
[Route("v1/ue")]
public class UniteEnseignementsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public UniteEnseignementsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduUniteEnseignements.AsNoTracking();
        if (Guid.TryParse(Request.Query["filiereId"].FirstOrDefault(), out var fid))
            query = query.Where(ue => ue.FiliereId == fid);
        if (Guid.TryParse(Request.Query["niveauId"].FirstOrDefault(), out var nid))
            query = query.Where(ue => ue.NiveauId == nid);
        var semestre = Request.Query["semestre"].FirstOrDefault();
        if (semestre is not null) query = query.Where(ue => ue.Semestre == semestre);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(ue => ue.Actif == actif);
        var (items, meta) = await query.ToContractPageAsync(opts, q => ue => ue.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var ue = await _ctx.EduUniteEnseignements.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("UniteEnseignement", id.ToString());
        return ApiResults.Ok(ue);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] UniteEnseignement dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduUniteEnseignements.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/ue/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UniteEnseignement dto, CancellationToken ct)
    {
        var ue = await _ctx.EduUniteEnseignements.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("UniteEnseignement", id.ToString());
        if (dto.Libelle is not null) ue.Libelle = dto.Libelle;
        if (dto.Credits > 0) ue.Credits = dto.Credits;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(ue);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ue = await _ctx.EduUniteEnseignements.FindAsync(new object[] { id }, ct)
                 ?? throw NotFoundException.For("UniteEnseignement", id.ToString());
        ue.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
