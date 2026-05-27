using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Subject;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/subjects")]
[Authorize]
public class SubjectController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<SubjectController> _logger;

    public SubjectController(ILogger<SubjectController> logger, ApiContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _context.Subjects
            .AsNoTracking()
            .Select(s => s.ToDto())
            .ToContractPageAsync(
                opts,
                searchPredicate: q => s => s.SubjectName.Contains(q),
                ct: ct);

        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var subject = await _context.Subjects.FindAsync(new object[] { id }, ct);
        if (subject is null) throw NotFoundException.For("Subject", id);
        return ApiResults.Ok(subject.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubjectDto dto, CancellationToken ct)
    {
        var subject = dto.ToEntity();
        subject.SubjectId = Guid.NewGuid().ToString();

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Subject {SubjectId} created", subject.SubjectId);

        return ApiResults.Created($"/v1/subjects/{subject.SubjectId}", subject.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SubjectDto dto, CancellationToken ct)
    {
        var subject = await _context.Subjects.FindAsync(new object[] { id }, ct);
        if (subject is null) throw NotFoundException.For("Subject", id);

        subject.SubjectName = dto.SubjectName;
        subject.Description = dto.Description;
        await _context.SaveChangesAsync(ct);

        return ApiResults.Ok(subject.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var subject = await _context.Subjects.FindAsync(new object[] { id }, ct);
        if (subject is null) throw NotFoundException.For("Subject", id);

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
