using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.School;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/schools")]
[Authorize]
public class SchoolController : ControllerBase
{
    private readonly ILogger<SchoolController> _logger;
    private readonly ApiContext _context;

    public SchoolController(ILogger<SchoolController> logger, ApiContext ctx)
    {
        _context = ctx;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _context.Schools
            .AsNoTracking()
            .Select(s => s.ToDto())
            .ToContractPageAsync(
                opts,
                searchPredicate: q => s => s.Name.Contains(q) || s.Description.Contains(q),
                ct: ct);

        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var school = await _context.Schools
            .Include(s => s.Periods)
            .Where(s => s.SchoolId == id)
            .Select(s => s.ToDto())
            .FirstOrDefaultAsync(ct);

        if (school is null) throw NotFoundException.For("School", id);
        return ApiResults.Ok(school);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SchoolDto dto, CancellationToken ct)
    {
        var withId = dto with { SchoolId = Guid.NewGuid().ToString() };
        var school = withId.ToEntity();
        school.Periods.AddRange(PeriodService.generatePeriods(dto, _logger)
            .Select(p => p.ToEntity())
            .ToList());

        _context.Schools.Add(school);
        await _context.SaveChangesAsync(ct);

        var location = $"/v1/schools/{school.SchoolId}";
        return ApiResults.Created(location, school.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SchoolUpdateDto dto, CancellationToken ct)
    {
        var school = await _context.Schools.FindAsync(new object[] { id }, ct);
        if (school is null) throw NotFoundException.For("School", id);

        school.Name = dto.Name;
        school.Description = dto.Description;
        school.SchoolType = dto.SchoolType;

        await _context.SaveChangesAsync(ct);
        return ApiResults.Ok(school.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var school = await _context.Schools.FindAsync(new object[] { id }, ct);
        if (school is null) throw NotFoundException.For("School", id);

        _context.Schools.Remove(school);
        await _context.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
