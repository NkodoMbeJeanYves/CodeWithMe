using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Grade;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Envelope;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/grades")]
public class GradesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public GradesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Enseignant, Role.DirecteurPedagogique, Role.Directeur)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Grades.AsNoTracking().Select(g => g.ToDto())
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpPost]
    [AllowRoles(Role.Enseignant)]
    public async Task<IActionResult> Create([FromBody] GradeCreateDto dto, CancellationToken ct)
    {
        ValidateGradeValue(dto.Value, dto.Scale);
        var entity = dto.ToEntity();
        _ctx.Grades.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/grades/{entity.Id}", entity.ToDto());
    }

    /// <summary>
    /// Saisie en lot — contrat section 9.3. Retourne 207 Multi-Status :
    /// les lignes valides sont créées, les invalides listées dans <c>failed[]</c>.
    /// </summary>
    [HttpPost("batch")]
    [AllowRoles(Role.Enseignant)]
    public async Task<IActionResult> Batch([FromBody] GradeBatchDto body, CancellationToken ct)
    {
        var teacherId = User.FindFirst("sub")?.Value ?? string.Empty;
        var created = 0;
        var failures = new List<GradeBatchFailure>();

        foreach (var item in body.Items)
        {
            if (item.Value < 0 || item.Value > body.Scale)
            {
                failures.Add(new GradeBatchFailure(item.StudentId, new[]
                {
                    new ApiError("VALIDATION_FAILED", $"value must be in [0, {body.Scale}]", "value")
                }));
                continue;
            }
            _ctx.Grades.Add(new Core.Models.Grade
            {
                Id = Guid.NewGuid().ToString(),
                StudentId = item.StudentId,
                SubjectId = body.SubjectId,
                TeacherId = teacherId,
                Value = item.Value,
                Scale = body.Scale,
                Coefficient = body.Coefficient,
                Type = body.Type,
                Period = body.Period,
                Comment = item.Comment
            });
            created++;
        }

        await _ctx.SaveChangesAsync(ct);

        return new ObjectResult(new GradeBatchResultDto(created, failures))
        {
            StatusCode = StatusCodes.Status207MultiStatus
        };
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.Enseignant)]
    public async Task<IActionResult> Update(string id, [FromBody] GradeCreateDto dto, CancellationToken ct)
    {
        var g = await _ctx.Grades.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Grade", id);
        if (g.PublishedAt is not null)
            throw new StateInvalidException("Published grades cannot be modified.");
        ValidateGradeValue(dto.Value, dto.Scale);
        g.Value = dto.Value;
        g.Comment = dto.Comment;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(g.ToDto());
    }

    [HttpPost("{id}/validate")]
    [AllowRoles(Role.DirecteurPedagogique)]
    public async Task<IActionResult> Validate(string id, CancellationToken ct)
    {
        var g = await _ctx.Grades.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Grade", id);
        g.ValidatedBy = User.FindFirst("sub")?.Value;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(g.ToDto());
    }

    /// <summary>
    /// Publie en masse les notes (par classe + période). Une fois publiées,
    /// elles ne sont plus PATCHables — contrat section 9.3.
    /// </summary>
    [HttpPost("publish")]
    [AllowRoles(Role.DirecteurPedagogique, Role.Directeur)]
    public async Task<IActionResult> Publish([FromBody] GradePublishRequestDto body, CancellationToken ct)
    {
        var students = await _ctx.Students.AsNoTracking()
            .Where(s => s.ClasseId == body.ClasseId)
            .Select(s => s.Id)
            .ToListAsync(ct);

        var grades = await _ctx.Grades
            .Where(g => g.Period == body.Period && students.Contains(g.StudentId) && g.PublishedAt == null)
            .ToListAsync(ct);

        var now = DateTime.UtcNow;
        foreach (var g in grades) g.PublishedAt = now;
        await _ctx.SaveChangesAsync(ct);

        return ApiResults.Ok(new GradePublishResultDto(grades.Count));
    }

    private static void ValidateGradeValue(double value, double scale)
    {
        if (value < 0 || value > scale)
            throw new BusinessRuleException(
                $"value must be in [0, {scale}]",
                field: "value",
                details: new { scale });
    }
}
