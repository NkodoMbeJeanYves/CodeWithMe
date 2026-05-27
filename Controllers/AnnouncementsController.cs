using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Communication;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/announcements")]
public class AnnouncementsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public AnnouncementsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<Announcement>().AsNoTracking()
            .Select(a => new AnnouncementDto(a.Title, a.Body, a.PublishedAt, a.ExpiresAt, a.Id))
            .ToContractPageAsync(opts, q => a => a.Title.Contains(q) || a.Body.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.ResponsableAdministratif, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] AnnouncementCreateDto dto, CancellationToken ct)
    {
        var ann = new Announcement
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Body = dto.Body,
            ExpiresAt = dto.ExpiresAt,
            PublishedAt = DateTime.UtcNow,
            PublishedBy = User.FindFirst("sub")?.Value
        };
        _ctx.Set<Announcement>().Add(ann);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/announcements/{ann.Id}",
            new AnnouncementDto(ann.Title, ann.Body, ann.PublishedAt, ann.ExpiresAt, ann.Id));
    }
}
