using CodeWithMe.Core.Dtos.Period;
using CodeWithMe.Core.Dtos.School;
using PeriodModel = CodeWithMe.Core.Models.Period;
using SchoolModel = CodeWithMe.Core.Models.School;

namespace CodeWithMe.Core.DataExtensions;

public static class DtoExtension
{
    // Period
    public static PeriodDto ToDto(this PeriodModel period) => new PeriodDto
    (
        period.PeriodType,
        period.Day,
        period.StartTime.AsString(),
        period.EndTime.AsString(),
        period.EventId,
        period.SchoolId,
        period.PeriodId
    );

    public static PeriodModel ToEntity(this PeriodDto dto) => new PeriodModel
    {
        Day = dto.Day,
        EndTime = dto.EndTime.AsTimeSpan(),
        EventId = dto.EventId,
        PeriodId = dto.PeriodId ?? "",
        SchoolId = dto.SchoolId,
        PeriodType = dto.Type,
        StartTime = dto.StartTime.AsTimeSpan(),
    };

    // School
    public static SchoolDto ToDto(this SchoolModel school) => new SchoolDto
    (
        school.Name,
        school.SchoolType.ToString(),
        school.Description,
        school.ClassStartTime.AsString(),
        school.ClassEndTime.AsString(),
        school.ClassDurationInMinutes,
        school.FirstBreakDurationInMinutes,
        school.FirstBreakStartTime.AsString(),
        school.SecondBreakDurationInMinutes,
        school.SecondBreakStartTime?.AsString(),
        school.ThirdBreakDurationInMinutes,
        school.ThirdBreakStartTime?.AsString(),
        school.SchoolId
    );

    public static SchoolModel ToEntity(this SchoolDto dto) => new SchoolModel
    {
        SchoolId = dto.SchoolId ?? "",
        Name = dto.Name,
        SchoolType = dto.SchoolType,
        Description = dto.Description,
        ClassStartTime = dto.ClassStartTime.AsTimeSpan(),
        ClassEndTime = dto.ClassEndTime.AsTimeSpan(),
        ClassDurationInMinutes = dto.ClassDurationInMinutes,
        FirstBreakDurationInMinutes = dto.FirstBreakDurationInMinutes,
        FirstBreakStartTime = dto.FirstBreakStartTime.AsTimeSpan(),
        SecondBreakDurationInMinutes = dto.SecondBreakDurationInMinutes,
        SecondBreakStartTime = dto.SecondBreakStartTime?.AsTimeSpan(),
        ThirdBreakDurationInMinutes = dto.ThirdBreakDurationInMinutes,
        ThirdBreakStartTime = dto.ThirdBreakStartTime?.AsTimeSpan(),
    };
}
