using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Finance;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/payroll")]
public class PayrollController : ControllerBase
{
    private readonly ApiContext _ctx;
    public PayrollController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet("periods")]
    [AllowRoles(Role.Comptable, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Periods(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<PayrollPeriod>().AsNoTracking()
            .Select(p => new PayrollPeriodDto(p.Year, p.Month, p.Status, p.Id))
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost("periods")]
    [AllowRoles(Role.Comptable)]
    public async Task<IActionResult> CreatePeriod([FromBody] PayrollPeriodCreateDto dto, CancellationToken ct)
    {
        if (dto.Month < 1 || dto.Month > 12)
            throw new BusinessRuleException("month must be in [1, 12]", field: "month");

        var p = new PayrollPeriod
        {
            Id = Guid.NewGuid().ToString(),
            Year = dto.Year,
            Month = dto.Month,
            Status = PayrollStatus.Open
        };
        _ctx.Set<PayrollPeriod>().Add(p);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/payroll/periods/{p.Id}", new PayrollPeriodDto(p.Year, p.Month, p.Status, p.Id));
    }

    [HttpPost("runs")]
    [AllowRoles(Role.Comptable)]
    public async Task<IActionResult> CreateRun([FromBody] PayrollRunCreateDto dto, CancellationToken ct)
    {
        var period = await _ctx.Set<PayrollPeriod>().FindAsync(new object[] { dto.PeriodId }, ct)
                     ?? throw NotFoundException.For("PayrollPeriod", dto.PeriodId);
        if (period.Status == PayrollStatus.Paid)
            throw new StateInvalidException("Period already paid; cannot run again.");

        var run = new PayrollRun
        {
            Id = Guid.NewGuid().ToString(),
            PeriodId = dto.PeriodId,
            RunAt = DateTime.UtcNow,
            RunBy = User.FindFirst("sub")?.Value,
            TotalAmount = 0
        };
        _ctx.Set<PayrollRun>().Add(run);
        period.Status = PayrollStatus.Locked;
        await _ctx.SaveChangesAsync(ct);

        return ApiResults.Created($"/v1/payroll/runs/{run.Id}",
            new PayrollRunDto(run.PeriodId, run.RunAt, run.RunBy, run.TotalAmount, run.Id));
    }
}
