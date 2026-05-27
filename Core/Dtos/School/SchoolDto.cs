using CodeWithMe.Core.Dtos.Period;

namespace CodeWithMe.Core.Dtos.School
{
    public record SchoolDto(
        string Name,
        string SchoolType,
        string Description,
        string ClassStartTime,
        string ClassEndTime,
        int ClassDurationInMinutes,
        int FirstBreakDurationInMinutes,
        string FirstBreakStartTime,
        int? SecondBreakDurationInMinutes = null,
        string? SecondBreakStartTime = null,
        int? ThirdBreakDurationInMinutes = null,
        string? ThirdBreakStartTime = null,
        string? SchoolId = null,
        ICollection<PeriodDto>? PeriodDtos = null
    );

    public record SchoolUpdateDto(
        string Name,
        string SchoolType,
        string Description
    );
}
