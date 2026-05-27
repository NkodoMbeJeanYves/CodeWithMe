using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Room;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/rooms")]
public class RoomsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public RoomsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Rooms.AsNoTracking().Select(r => r.ToDto())
            .ToContractPageAsync(opts, q => r => r.Name.Contains(q) || r.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var r = await _ctx.Rooms.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Room", id);
        return ApiResults.Ok(r.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Create([FromBody] RoomCreateDto dto, CancellationToken ct)
    {
        var entity = dto.ToEntity();
        _ctx.Rooms.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/rooms/{entity.Id}", entity.ToDto());
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.Directeur)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var r = await _ctx.Rooms.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Room", id);
        _ctx.Rooms.Remove(r);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
