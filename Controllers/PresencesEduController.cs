using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/presences")]
public class PresencesEduController : ControllerBase
{
    private readonly ApiContext _ctx;
    public PresencesEduController(ApiContext ctx) { _ctx = ctx; }

    [HttpPost("saisir")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Saisir([FromBody] List<Presence> dtos, CancellationToken ct)
    {
        foreach (var dto in dtos)
        {
            var existing = await _ctx.EduPresences
                .FirstOrDefaultAsync(p => p.SeanceId == dto.SeanceId && p.ApprenantId == dto.ApprenantId, ct);
            if (existing is not null)
            {
                existing.Statut = dto.Statut;
                existing.MinutesRetard = dto.MinutesRetard;
                existing.Remarque = dto.Remarque;
            }
            else
            {
                dto.Id = Guid.NewGuid();
                _ctx.EduPresences.Add(dto);
            }
        }
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(new { Saved = dtos.Count });
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.Enseignant, Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Presence dto, CancellationToken ct)
    {
        var p = await _ctx.EduPresences.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Presence", id.ToString());
        if (dto.Statut is not null) p.Statut = dto.Statut;
        if (dto.MinutesRetard.HasValue) p.MinutesRetard = dto.MinutesRetard;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(p);
    }
}
