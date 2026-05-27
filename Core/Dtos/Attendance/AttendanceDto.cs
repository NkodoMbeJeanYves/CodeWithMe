using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Attendance;

public sealed record AttendanceDto(
    string SessionId,
    string StudentId,
    AttendanceStatus Status,
    int? MinutesLate,
    string? Justification,
    string? DocumentId,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);
