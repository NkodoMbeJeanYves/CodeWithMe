using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Session;

public sealed record SessionDto(
    string SubjectId,
    string TeacherId,
    string ClasseId,
    string? RoomId,
    DateTime StartAt,
    DateTime EndAt,
    SessionType Type,
    SessionStatus Status,
    SessionAttendanceSummary Attendance,
    string? Notes,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record SessionAttendanceSummary(bool Recorded, int PresentCount, int AbsentCount);

public sealed record SessionCreateDto(
    string SubjectId,
    string TeacherId,
    string ClasseId,
    DateTime StartAt,
    DateTime EndAt,
    string? RoomId = null,
    SessionType Type = SessionType.Lecture,
    string? Notes = null);

public sealed record SessionUpdateDto(
    string? RoomId = null,
    DateTime? StartAt = null,
    DateTime? EndAt = null,
    SessionStatus? Status = null,
    string? Notes = null);

public sealed record SessionCancelDto(string? Reason = null);

public sealed record AttendanceRecordInput(
    string StudentId,
    AttendanceStatus Status,
    int? MinutesLate = null,
    string? Justification = null);

public sealed record AttendanceBatchDto(IReadOnlyList<AttendanceRecordInput> Records);

public sealed record AttendanceBatchSummary(int Present, int Absent, int Late, int Excused);

public sealed record AttendanceJustifyDto(string Justification, string? DocumentId = null);
