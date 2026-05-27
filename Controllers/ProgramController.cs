using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Program;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/programs")]
[Authorize]
public class ProgramController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProgramController> _logger;

    public ProgramController(ILogger<ProgramController> logger, ApiContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Liste paginée des programmes. Filtre par école via <c>?filter[schoolId]=...</c>.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _context.Programs
            .AsNoTracking()
            .Select(p => p.ToDto())
            .ToContractPageAsync(
                opts,
                searchPredicate: q => p => p.Name.Contains(q),
                ct: ct);

        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var program = await _context.Programs.FindAsync(new object[] { id }, ct);
        if (program is null) throw NotFoundException.For("Program", id);
        return ApiResults.Ok(program.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProgramDto dto, CancellationToken ct)
    {
        var program = dto.ToEntity();
        program.Program_id = Guid.NewGuid().ToString();

        _context.Programs.Add(program);
        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Program {ProgramId} created", program.Program_id);

        return ApiResults.Created($"/v1/programs/{program.Program_id}", program.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] ProgramDto dto, CancellationToken ct)
    {
        var program = await _context.Programs.FindAsync(new object[] { id }, ct);
        if (program is null) throw NotFoundException.For("Program", id);

        program.Name = dto.Name;
        await _context.SaveChangesAsync(ct);

        return ApiResults.Ok(program.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var program = await _context.Programs.FindAsync(new object[] { id }, ct);
        if (program is null) throw NotFoundException.For("Program", id);

        _context.Programs.Remove(program);
        await _context.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
