using CodeWithMe.Core.Models;

namespace CodeWithMe.Core.Dtos.School
{
    public record UpdateSchoolDto(
        string SchoolId,
        string Name,
        SchoolTypes SchoolType,
        string Description,
        string ClassStartTime,
        string ClassEndTime,
        int ClassDurationInMinutes,
        int FirstBreakDurationInMinutes,
        string FirstBreakStartTime,
        int? SecondBreakDurationInMinutes = null,
        string? SecondBreakStartTime = null,
        int? ThirdBreakDurationInMinutes = null,
        string? ThirdBreakStartTime = null
    );
}
