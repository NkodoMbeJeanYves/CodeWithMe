using CodeWithMe.Core.DataExtensions;
using PeriodModel = CodeWithMe.Core.Models.Period;
namespace CodeWithMe.Core.Dtos.Period
{
    public record PeriodDto(
        string PeriodType,
        int Day,
        string StartTime,
        string EndTime,
        string? EventId,
        string? SchoolId,
        string? PeriodId
    );

    public static class PeriodDtoExtensions
    {
        public static PeriodDto ToDto(this PeriodModel period) => new PeriodDto
            (
                period.PeriodType,
                period.Day,
                period.StartTime.ToString(@"hh\:mm"),
                period.EndTime.ToString(@"hh\:mm"),
                period.EventId,
                period.SchoolId,
                period.PeriodId
            );

        public static PeriodModel ToEntity(this PeriodDto dto) => new PeriodModel
        {
            Day = dto.Day,
            EndTime = dto.EndTime.AsTimeSpan(),
            EventId = dto.EventId,
            PeriodId = dto.PeriodId,
            SchoolId = dto.SchoolId,
            PeriodType = dto.PeriodType,
            StartTime = dto.StartTime.AsTimeSpan(),
        };
    }

}



