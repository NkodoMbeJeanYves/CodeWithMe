using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Grade;
using CodeWithMe.Core.Dtos.Student;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/students")]
public class StudentsController : ControllerBase
{
    private readonly ApiContext _ctx;
    public StudentsController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.Secretaire, Role.Enseignant)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Students.AsNoTracking().Select(s => s.ToDto())
            .ToContractPageAsync(opts, q => s => s.Matricule.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var s = await _ctx.Students.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Student", id);
        return ApiResults.Ok(s.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Create([FromBody] StudentCreateDto dto, CancellationToken ct)
    {
        var entity = dto.ToEntity();
        _ctx.Students.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/students/{entity.Id}", entity.ToDto());
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> Update(string id, [FromBody] StudentUpdateDto dto, CancellationToken ct)
    {
        var s = await _ctx.Students.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Student", id);

        if (dto.ClasseId is not null) s.ClasseId = dto.ClasseId;
        if (dto.FiliereId is not null) s.FiliereId = dto.FiliereId;
        if (dto.Status is not null) s.Status = dto.Status.Value;

        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(s.ToDto());
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.Directeur)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var s = await _ctx.Students.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Student", id);
        // Soft : archive plutôt que delete physique (contrat 4.2).
        s.Status = StudentStatus.Transferred;
        s.DeletedAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }

    [HttpGet("{id}/grades")]
    public async Task<IActionResult> Grades(string id, CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Grades.AsNoTracking()
            .Where(g => g.StudentId == id)
            .Select(g => g.ToDto())
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}/attendance")]
    public async Task<IActionResult> Attendance(
        string id,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken ct)
    {
        var query = _ctx.Attendances.AsNoTracking()
            .Where(a => a.StudentId == id);
        if (from is not null) query = query.Where(a => a.CreatedAt >= from);
        if (to is not null) query = query.Where(a => a.CreatedAt <= to);

        var data = await query.Select(a => a.ToDto()).ToListAsync(ct);
        var totals = new
        {
            present = data.Count(a => a.Status == AttendanceStatus.Present),
            absent = data.Count(a => a.Status == AttendanceStatus.Absent),
            late = data.Count(a => a.Status == AttendanceStatus.Late),
            excused = data.Count(a => a.Status == AttendanceStatus.Excused)
        };
        return ApiResults.Ok(new { data, meta = new { totals } });
    }

    /// <summary>
    /// Bulletin trimestriel — calcul à la volée (V1).
    /// </summary>
    [HttpGet("{id}/bulletin")]
    public async Task<IActionResult> Bulletin(string id, [FromQuery] GradePeriod period, CancellationToken ct)
    {
        var student = await _ctx.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct)
                      ?? throw NotFoundException.For("Student", id);

        var classGrades = await _ctx.Grades.AsNoTracking()
            .Where(g => g.Period == period)
            .Join(_ctx.Students.AsNoTracking(),
                  g => g.StudentId,
                  s => s.Id,
                  (g, s) => new { g, s.ClasseId })
            .Where(x => x.ClasseId == student.ClasseId)
            .Select(x => x.g)
            .ToListAsync(ct);

        var subjects = classGrades
            .GroupBy(g => g.SubjectId)
            .Select(grp =>
            {
                var studentGrades = grp.Where(g => g.StudentId == id).ToList();
                var studentAvg = WeightedAverage(studentGrades);
                var classAvg = WeightedAverage(grp.ToList());
                var coefficient = studentGrades.FirstOrDefault()?.Coefficient ?? 1;

                // Rang par moyenne décroissante (V1 : O(n²) acceptable).
                var ranking = grp.GroupBy(x => x.StudentId)
                    .Select(g => new { StudentId = g.Key, Avg = WeightedAverage(g.ToList()) })
                    .OrderByDescending(x => x.Avg)
                    .Select((x, i) => new { x.StudentId, Rank = i + 1 })
                    .FirstOrDefault(x => x.StudentId == id);

                return new BulletinSubjectDto(
                    grp.Key,
                    studentAvg,
                    classAvg,
                    ranking?.Rank ?? 0,
                    coefficient,
                    studentGrades.Select(g => g.ToDto()).ToList());
            })
            .ToList();

        var overall = new BulletinOverallDto(
            subjects.Count == 0 ? 0 : subjects.Average(s => s.Average),
            subjects.Count == 0 ? 0 : subjects.Average(s => s.ClassAverage),
            0,
            classGrades.Select(g => g.StudentId).Distinct().Count()
        );

        var publishedAt = classGrades.Where(g => g.StudentId == id).Max(g => (DateTime?)g.PublishedAt);
        var bulletin = new BulletinDto(id, period, student.ClasseId, subjects, overall, publishedAt);
        return ApiResults.Ok(bulletin);
    }

    private static double WeightedAverage(IList<Core.Models.Grade> grades)
    {
        if (grades.Count == 0) return 0;
        var totalCoef = grades.Sum(g => g.Coefficient);
        if (totalCoef == 0) return 0;
        return Math.Round(grades.Sum(g => g.Value * g.Coefficient) / totalCoef, 2);
    }
}
