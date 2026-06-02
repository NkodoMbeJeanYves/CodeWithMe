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
[Route("v1/classes-edu")]
public class ClassesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public ClassesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduClasses.AsNoTracking();
        if (Guid.TryParse(Request.Query["etablissementId"].FirstOrDefault(), out var eid))
            query = query.Where(c => c.EtablissementId == eid);
        if (Guid.TryParse(Request.Query["anneeAcademiqueId"].FirstOrDefault(), out var aid))
            query = query.Where(c => c.AnneeAcademiqueId == aid);
        if (Guid.TryParse(Request.Query["niveauId"].FirstOrDefault(), out var nid))
            query = query.Where(c => c.NiveauId == nid);
        var statut = Request.Query["statut"].FirstOrDefault();
        if (statut is not null) query = query.Where(c => c.Statut == statut);
        var (items, meta) = await query.ToContractPageAsync(opts, q => c => c.Libelle.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduClasses.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ClasseEdu", id.ToString());
        return ApiResults.Ok(c);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] ClasseEdu dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduClasses.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/classes-edu/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ClasseEdu dto, CancellationToken ct)
    {
        var c = await _ctx.EduClasses.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ClasseEdu", id.ToString());
        if (dto.Libelle is not null) c.Libelle = dto.Libelle;
        if (dto.Statut is not null) c.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(c);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduClasses.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ClasseEdu", id.ToString());
        c.Statut = "inactive";
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
