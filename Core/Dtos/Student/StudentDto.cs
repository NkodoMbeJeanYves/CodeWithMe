using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Student;

public sealed record StudentDto(
    string UserId,
    string Matricule,
    string ClasseId,
    string? FiliereId,
    DateTime? DateOfBirth,
    Gender? Gender,
    DateTime? EnrolledAt,
    StudentStatus Status,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record StudentCreateDto(
    string UserId,
    string Matricule,
    string ClasseId,
    string? FiliereId = null,
    DateTime? DateOfBirth = null,
    Gender? Gender = null,
    DateTime? EnrolledAt = null,
    StudentStatus Status = StudentStatus.Enrolled);

public sealed record StudentUpdateDto(
    string? ClasseId = null,
    string? FiliereId = null,
    StudentStatus? Status = null);
