using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Teacher;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/teachers")]
public class TeachersController : ControllerBase
{
    private readonly ApiContext _ctx;
    public TeachersController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.ResponsableAdministratif)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Teachers.AsNoTracking().Select(t => t.ToDto())
            .ToContractPageAsync(opts, q => t => t.Matricule.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var t = await _ctx.Teachers.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Teacher", id);
        return ApiResults.Ok(t.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.ResponsableAdministratif)]
    public async Task<IActionResult> Create([FromBody] TeacherCreateDto dto, CancellationToken ct)
    {
        var entity = dto.ToEntity();
        _ctx.Teachers.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/teachers/{entity.Id}", entity.ToDto());
    }

    /// <summary>
    /// Sessions assignées à l'enseignant — contrat section 4.2.
    /// </summary>
    [HttpGet("{id}/sessions")]
    public async Task<IActionResult> Sessions(string id, CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.CourseSessions.AsNoTracking()
            .Where(s => s.TeacherId == id)
            .Select(s => s.ToDto())
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    /// <summary>
    /// EDT enseignant sur une semaine — <c>?weekOf=2026-05-25</c>.
    /// </summary>
    [HttpGet("{id}/timetable")]
    public async Task<IActionResult> Timetable(string id, [FromQuery] DateTime? weekOf, CancellationToken ct)
    {
        var weekStart = weekOf?.Date ?? StartOfIsoWeek(DateTime.UtcNow);
        var weekEnd = weekStart.AddDays(7);

        var sessions = await _ctx.CourseSessions.AsNoTracking()
            .Where(s => s.TeacherId == id && s.StartAt >= weekStart && s.StartAt < weekEnd)
            .OrderBy(s => s.StartAt)
            .Select(s => s.ToDto())
            .ToListAsync(ct);

        return ApiResults.Ok(new { teacherId = id, weekOf = weekStart, sessions });
    }

    private static DateTime StartOfIsoWeek(DateTime dt)
    {
        var diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dt.AddDays(-diff).Date;
    }
}
