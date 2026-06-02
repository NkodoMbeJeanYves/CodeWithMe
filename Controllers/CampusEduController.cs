using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/campus")]
public class CampusEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public CampusEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduCampus.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Campus", id.ToString());
        return ApiResults.Ok(c);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] Campus dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduCampus.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/campus/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Campus dto, CancellationToken ct)
    {
        var c = await _ctx.EduCampus.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Campus", id.ToString());
        if (dto.Nom is not null) c.Nom = dto.Nom;
        if (dto.Adresse is not null) c.Adresse = dto.Adresse;
        if (dto.Ville is not null) c.Ville = dto.Ville;
        c.TelephoneDirecteur = dto.TelephoneDirecteur ?? c.TelephoneDirecteur;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(c);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var c = await _ctx.EduCampus.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Campus", id.ToString());
        c.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
