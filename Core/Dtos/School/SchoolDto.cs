namespace CodeWithMe.Core.Dtos.School
{
    public record SchoolDto(string Name, string SchoolType, string Description, TimeSpan ClassStartTime, TimeSpan ClassEndTime, int ClassDurationInMinutes, int FirstBreakDurationInMinutes, TimeSpan FirstBreakStartTime, int? SecondBreakDurationInMinutes = null, TimeSpan? SecondBreakStartTime = null, int? ThirdBreakDurationInMinutes = null, TimeSpan? ThirdBreakStartTime = null, string? SchoolId = null);

    public static class SchoolDtoExtensions
    {
        public static SchoolDto ToDto(this Models.School school)
        {
            return new SchoolDto
            (
                school.Name,
                school.SchoolType,
                school.Description,
                school.ClassStartTime,
                school.ClassEndTime,
                school.ClassDurationInMinutes,
                school.FirstBreakDurationInMinutes,
                school.FirstBreakStartTime,
                school.SecondBreakDurationInMinutes,
                school.SecondBreakStartTime,
                school.ThirdBreakDurationInMinutes,
                school.ThirdBreakStartTime,
                school.SchoolId
            );
        }

        public static Models.School ToEntity(this SchoolDto dto)
        {
            return new Models.School
            {
                SchoolId = dto.SchoolId ?? "",
                Name = dto.Name,
                SchoolType = dto.SchoolType,
                Description = dto.Description,
                ClassStartTime = dto.ClassStartTime,
                ClassEndTime = dto.ClassEndTime,
                ClassDurationInMinutes = dto.ClassDurationInMinutes,
                FirstBreakDurationInMinutes = dto.FirstBreakDurationInMinutes,
                FirstBreakStartTime = dto.FirstBreakStartTime,
                SecondBreakDurationInMinutes = dto.SecondBreakDurationInMinutes,
                SecondBreakStartTime = dto.SecondBreakStartTime,
                ThirdBreakDurationInMinutes = dto.ThirdBreakDurationInMinutes,
                ThirdBreakStartTime = dto.ThirdBreakStartTime,
            };
        }
    }
}
