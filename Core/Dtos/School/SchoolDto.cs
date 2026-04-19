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
        string? SchoolId = null
    );

    public static class SchoolDtoExtensions
    {
        public static SchoolDto ToDto(this Models.School school) => new SchoolDto
            (
                school.Name,
                school.SchoolType,
                school.Description,
                school.ClassStartTime.ToString(@"hh\:mm"),
                school.ClassEndTime.ToString(@"hh\:mm"),
                school.ClassDurationInMinutes,
                school.FirstBreakDurationInMinutes,
                school.FirstBreakStartTime.ToString(@"hh\:mm"),
                school.SecondBreakDurationInMinutes,
                school.SecondBreakStartTime?.ToString(@"hh\:mm"),
                school.ThirdBreakDurationInMinutes,
                school.ThirdBreakStartTime?.ToString(@"hh\:mm"),
                school.SchoolId
            );

        public static Models.School ToEntity(this SchoolDto dto) => new Models.School
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

        public static TimeSpan AsTimeSpan(this string str)
        {
            TimeSpan value;
            if (TimeSpan.TryParseExact(str, "hh\\:mm", null, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException($"Unable to Parse {str}", str);
            }
        }
    }
}
