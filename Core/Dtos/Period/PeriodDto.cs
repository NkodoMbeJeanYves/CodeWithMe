namespace CodeWithMe.Core.Dtos.Period
{
    public record PeriodDto(
        string Type,
        int Day,
        string StartTime,
        string EndTime,
        string? EventId,
        string? SchoolId,
        string? PeriodId
    );
}



