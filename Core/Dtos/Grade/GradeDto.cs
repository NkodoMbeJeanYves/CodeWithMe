using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Grade;

public sealed record GradeDto(
    string StudentId,
    string SubjectId,
    string TeacherId,
    string? SessionId,
    double Value,
    double Scale,
    double Coefficient,
    GradeType Type,
    GradePeriod Period,
    string? Comment,
    string? ValidatedBy,
    DateTime? PublishedAt,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record GradeCreateDto(
    string StudentId,
    string SubjectId,
    string TeacherId,
    double Value,
    double Scale,
    double Coefficient,
    GradeType Type,
    GradePeriod Period,
    string? SessionId = null,
    string? Comment = null);

public sealed record GradeBatchItemDto(string StudentId, double Value, string? Comment = null);

public sealed record GradeBatchDto(
    string ClasseId,
    string SubjectId,
    GradePeriod Period,
    GradeType Type,
    double Scale,
    double Coefficient,
    IReadOnlyList<GradeBatchItemDto> Items);

public sealed record GradeBatchResultDto(
    int Created,
    IReadOnlyList<GradeBatchFailure> Failed);

public sealed record GradeBatchFailure(string StudentId, IReadOnlyList<Models.Envelope.ApiError> Errors);

public sealed record GradePublishRequestDto(string ClasseId, GradePeriod Period);

public sealed record GradePublishResultDto(int PublishedCount);

public sealed record BulletinSubjectDto(
    string SubjectId,
    double Average,
    double ClassAverage,
    int Rank,
    double Coefficient,
    IReadOnlyList<GradeDto> Grades);

public sealed record BulletinOverallDto(double Average, double ClassAverage, int Rank, int TotalStudents);

public sealed record BulletinDto(
    string StudentId,
    GradePeriod Period,
    string ClasseId,
    IReadOnlyList<BulletinSubjectDto> Subjects,
    BulletinOverallDto Overall,
    DateTime? PublishedAt);
