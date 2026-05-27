using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Timetable;

public sealed record TimetableEntryDto(
    int DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime,
    string? SessionId,
    string? Color);

public sealed record TimetableDto(
    TimetableOwnerType OwnerType,
    string OwnerId,
    DateTime WeekOf,
    IReadOnlyList<TimetableEntryDto> Entries,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);
