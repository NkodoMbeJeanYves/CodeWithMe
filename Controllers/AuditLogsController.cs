using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Admin;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/audit-logs")]
public class AuditLogsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public AuditLogsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Directeur, Role.SuperAdmin)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<Core.Models.AuditLog>().AsNoTracking()
            .Select(a => new AuditLogDto(a.Id, a.TenantId, a.UserId, a.Action, a.ResourceType, a.ResourceId, a.IpAddress, a.OccurredAt))
            .ToContractPageAsync(opts, q => a => a.Action.Contains(q) || a.ResourceType.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }
}
