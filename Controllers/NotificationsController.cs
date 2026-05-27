using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Communication;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public NotificationsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var sub = User.FindFirst("sub")?.Value ?? string.Empty;
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<Notification>().AsNoTracking()
            .Where(n => n.UserId == sub)
            .Select(n => new NotificationDto(n.UserId, n.Type, n.Title, n.Body, n.ReadAt, n.CreatedAt, n.Id))
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkRead(string id, CancellationToken ct)
    {
        var n = await _ctx.Set<Notification>().FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Notification", id);
        n.ReadAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(new NotificationDto(n.UserId, n.Type, n.Title, n.Body, n.ReadAt, n.CreatedAt, n.Id));
    }
}
