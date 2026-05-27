using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Session;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/attendances")]
public class AttendancesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public AttendancesController(ApiContext ctx) { _ctx = ctx; }

    /// <summary>
    /// Justifie une absence — contrat section 9.4. Passe le statut à <c>excused</c>.
    /// </summary>
    [HttpPost("{id}/justify")]
    [AllowRoles(Role.Secretaire, Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Justify(string id, [FromBody] AttendanceJustifyDto dto, CancellationToken ct)
    {
        var a = await _ctx.Attendances.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Attendance", id);
        a.Justification = dto.Justification;
        a.DocumentId = dto.DocumentId;
        a.Status = AttendanceStatus.Excused;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(a.ToDto());
    }
}
