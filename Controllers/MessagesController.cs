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
[Route("v1/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public MessagesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var sub = User.FindFirst("sub")?.Value ?? string.Empty;
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<Message>().AsNoTracking()
            .Where(m => m.ToUserId == sub || m.FromUserId == sub)
            .Select(m => new MessageDto(m.FromUserId, m.ToUserId, m.Subject, m.Body, m.ReadAt, m.CreatedAt, m.Id))
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] MessageCreateDto dto, CancellationToken ct)
    {
        var fromUser = User.FindFirst("sub")?.Value
                       ?? throw new UnauthenticatedException();
        var msg = new Message
        {
            Id = Guid.NewGuid().ToString(),
            FromUserId = fromUser,
            ToUserId = dto.ToUserId,
            Subject = dto.Subject,
            Body = dto.Body
        };
        _ctx.Set<Message>().Add(msg);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/messages/{msg.Id}",
            new MessageDto(msg.FromUserId, msg.ToUserId, msg.Subject, msg.Body, msg.ReadAt, msg.CreatedAt, msg.Id));
    }
}
