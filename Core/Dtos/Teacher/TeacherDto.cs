using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Teacher;

public sealed record TeacherDto(
    string UserId,
    string Matricule,
    DateTime? HireDate,
    ContractType ContractType,
    int WeeklyHours,
    TeacherStatus Status,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record TeacherCreateDto(
    string UserId,
    string Matricule,
    DateTime? HireDate = null,
    ContractType ContractType = ContractType.Permanent,
    int WeeklyHours = 0,
    TeacherStatus Status = TeacherStatus.Active);
