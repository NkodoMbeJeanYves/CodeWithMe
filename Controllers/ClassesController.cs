using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Classe;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/classes")]
public class ClassesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public ClassesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Classes.AsNoTracking().Select(c => c.ToDto())
            .ToContractPageAsync(opts, q => c => c.Name.Contains(q) || c.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var c = await _ctx.Classes.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Classe", id);
        return ApiResults.Ok(c.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] ClasseCreateDto dto, CancellationToken ct)
    {
        var entity = dto.ToEntity();
        _ctx.Classes.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/classes/{entity.Id}", entity.ToDto());
    }

    /// <summary>
    /// Roster des apprenants d'une classe — contrat section 4.2.
    /// </summary>
    [HttpGet("{id}/students")]
    public async Task<IActionResult> Students(string id, CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Students.AsNoTracking()
            .Where(s => s.ClasseId == id)
            .Select(s => s.ToDto())
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    /// <summary>
    /// EDT de la classe pour une semaine donnée — <c>?weekOf=2026-05-25</c>.
    /// </summary>
    [HttpGet("{id}/timetable")]
    public async Task<IActionResult> Timetable(string id, [FromQuery] DateTime? weekOf, CancellationToken ct)
    {
        var weekStart = weekOf?.Date ?? StartOfIsoWeek(DateTime.UtcNow);
        var weekEnd = weekStart.AddDays(7);

        var sessions = await _ctx.CourseSessions
            .AsNoTracking()
            .Where(s => s.ClasseId == id && s.StartAt >= weekStart && s.StartAt < weekEnd)
            .OrderBy(s => s.StartAt)
            .Select(s => s.ToDto())
            .ToListAsync(ct);

        return ApiResults.Ok(new { classeId = id, weekOf = weekStart, sessions });
    }

    private static DateTime StartOfIsoWeek(DateTime dt)
    {
        var diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dt.AddDays(-diff).Date;
    }
}
