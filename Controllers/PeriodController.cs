using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Period;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/periods")]
[Authorize]
public class PeriodController : ControllerBase
{
    private readonly ILogger<PeriodController> _logger;
    private readonly ApiContext _context;

    public PeriodController(ILogger<PeriodController> logger, ApiContext ctx)
    {
        _logger = logger;
        _context = ctx;
    }

    /// <summary>
    /// Liste paginée des périodes. Filtre l'école via <c>?filter[schoolId]=...</c>.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _context.Periods
            .AsNoTracking()
            .Select(p => p.ToDto())
            .ToContractPageAsync(opts, ct: ct);

        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var period = await _context.Periods.FindAsync(new object[] { id }, ct);
        if (period is null) throw NotFoundException.For("Period", id);
        return ApiResults.Ok(period.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PeriodDto dto, CancellationToken ct)
    {
        var period = dto.ToEntity();
        period.PeriodId = Guid.NewGuid().ToString();

        _context.Periods.Add(period);
        await _context.SaveChangesAsync(ct);

        return ApiResults.Created($"/v1/periods/{period.PeriodId}", period.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] PeriodDto dto, CancellationToken ct)
    {
        if (dto.PeriodId is not null && id != dto.PeriodId)
            throw new BusinessRuleException("Route id and payload id mismatch.", field: "periodId");

        var period = dto.ToEntity();
        period.PeriodId = id;
        _context.Entry(period).State = EntityState.Modified;
        await _context.SaveChangesAsync(ct);

        return ApiResults.Ok(period.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var period = await _context.Periods.FindAsync(new object[] { id }, ct);
        if (period is null) throw NotFoundException.For("Period", id);

        _context.Periods.Remove(period);
        await _context.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
