using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/calendrier")]
public class CalendrierController : ControllerBase
{
    private readonly ApiContext _ctx;
    public CalendrierController(ApiContext ctx) { _ctx = ctx; }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] EvenementCalendrier dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduEvenementsCalendrier.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/calendrier/{dto.Id}", dto);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var e = await _ctx.EduEvenementsCalendrier.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("EvenementCalendrier", id.ToString());
        _ctx.EduEvenementsCalendrier.Remove(e);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
