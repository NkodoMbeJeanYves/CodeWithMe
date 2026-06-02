using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/annees-academiques")]
public class AnneesAcademiquesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public AnneesAcademiquesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduAnneesAcademiques.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("AnneeAcademique", id.ToString());
        return ApiResults.Ok(a);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] AnneeAcademique dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduAnneesAcademiques.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/annees-academiques/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] AnneeAcademique dto, CancellationToken ct)
    {
        var a = await _ctx.EduAnneesAcademiques.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("AnneeAcademique", id.ToString());
        if (dto.Libelle is not null) a.Libelle = dto.Libelle;
        if (dto.Statut is not null) a.Statut = dto.Statut;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(a);
    }

    [HttpPatch("{id}/activer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Activer(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduAnneesAcademiques.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("AnneeAcademique", id.ToString());
        a.Statut = "en_cours";
        a.Active = true;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(a);
    }

    [HttpPatch("{id}/cloturer")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Cloturer(Guid id, CancellationToken ct)
    {
        var a = await _ctx.EduAnneesAcademiques.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("AnneeAcademique", id.ToString());
        a.Statut = "cloturee";
        a.Active = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(a);
    }
}
