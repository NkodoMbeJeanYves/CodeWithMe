using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Session;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/sessions")]
public class SessionsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public SessionsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.CourseSessions.AsNoTracking().Select(s => s.ToDto())
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var s = await _ctx.CourseSessions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Session", id);
        return ApiResults.Ok(s.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] SessionCreateDto dto, CancellationToken ct)
    {
        if (dto.EndAt <= dto.StartAt)
            throw new BusinessRuleException("endAt must be after startAt", field: "endAt");

        var entity = dto.ToEntity();
        _ctx.CourseSessions.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/sessions/{entity.Id}", entity.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] SessionUpdateDto dto, CancellationToken ct)
    {
        var s = await _ctx.CourseSessions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Session", id);

        if (dto.RoomId is not null) s.RoomId = dto.RoomId;
        if (dto.StartAt is not null) s.StartAt = dto.StartAt.Value;
        if (dto.EndAt is not null) s.EndAt = dto.EndAt.Value;
        if (dto.Status is not null) AssertTransition(s.Status, dto.Status.Value).Apply(s);
        if (dto.Notes is not null) s.Notes = dto.Notes;

        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s.ToDto());
    }

    [HttpPost("{id}/cancel")]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.Enseignant)]
    public async Task<IActionResult> Cancel(string id, [FromBody] SessionCancelDto? body, CancellationToken ct)
    {
        var s = await _ctx.CourseSessions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Session", id);

        AssertTransition(s.Status, SessionStatus.Cancelled).Apply(s);
        s.CancelReason = body?.Reason;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s.ToDto());
    }

    /// <summary>
    /// Saisie de présences en lot pour une session — contrat section 9.2.
    /// </summary>
    [HttpPost("{id}/attendance")]
    [AllowRoles(Role.Enseignant, Role.Surveillant)]
    public async Task<IActionResult> RecordAttendance(
        string id, [FromBody] AttendanceBatchDto body, CancellationToken ct)
    {
        var s = await _ctx.CourseSessions.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Session", id);

        // Upsert : on remplace les rows existantes par session+student.
        var existing = await _ctx.Attendances
            .Where(a => a.SessionId == id)
            .ToDictionaryAsync(a => a.StudentId, ct);

        foreach (var input in body.Records)
        {
            if (existing.TryGetValue(input.StudentId, out var row))
            {
                row.Status = input.Status;
                row.MinutesLate = input.MinutesLate;
                row.Justification = input.Justification;
            }
            else
            {
                _ctx.Attendances.Add(new Core.Models.Attendance
                {
                    Id = Guid.NewGuid().ToString(),
                    SessionId = id,
                    StudentId = input.StudentId,
                    Status = input.Status,
                    MinutesLate = input.MinutesLate,
                    Justification = input.Justification
                });
            }
        }

        s.AttendanceRecorded = true;
        s.PresentCount = body.Records.Count(r => r.Status == AttendanceStatus.Present);
        s.AbsentCount = body.Records.Count(r => r.Status == AttendanceStatus.Absent);

        await _ctx.SaveChangesAsync(ct);

        return ApiResults.Ok(new AttendanceBatchSummary(
            Present: body.Records.Count(r => r.Status == AttendanceStatus.Present),
            Absent: body.Records.Count(r => r.Status == AttendanceStatus.Absent),
            Late: body.Records.Count(r => r.Status == AttendanceStatus.Late),
            Excused: body.Records.Count(r => r.Status == AttendanceStatus.Excused)
        ));
    }

    // ---- State machine — contrat section 9.2 ----

    private record TransitionResult(SessionStatus Target)
    {
        public void Apply(Core.Models.CourseSession s) => s.Status = Target;
    }

    private static TransitionResult AssertTransition(SessionStatus current, SessionStatus target)
    {
        bool ok = (current, target) switch
        {
            (SessionStatus.Scheduled, SessionStatus.InProgress) => true,
            (SessionStatus.Scheduled, SessionStatus.Cancelled) => true,
            (SessionStatus.InProgress, SessionStatus.Done) => true,
            (SessionStatus.InProgress, SessionStatus.Cancelled) => true,
            _ when current == target => true,
            _ => false
        };
        if (!ok)
            throw new StateInvalidException(
                $"Cannot transition session from {current} to {target}.",
                new { from = current.ToString(), to = target.ToString() });
        return new TransitionResult(target);
    }
}
