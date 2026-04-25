using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Period;
using CodeWithMe.Core.Dtos.School;
using System.Text.Json;

namespace CodeWithMe.Core.Services;

public record Break(string StartTime, int Duration);

public static class PeriodService
{
    public const string COURSE = "COURSE";
    public const string BREAK = "BREAK";
    private static List<Break> BuildBreaks(SchoolDto dto)
    {
        var breaks = new List<Break>
        {
            new Break(dto.FirstBreakStartTime, dto.FirstBreakDurationInMinutes)
        };

        if (dto.SecondBreakStartTime != null)
        {
            breaks.Add(new Break(dto.SecondBreakStartTime, dto.SecondBreakDurationInMinutes ?? 0));
        }

        if (dto.ThirdBreakStartTime != null)
        {
            breaks.Add(new Break(dto.ThirdBreakStartTime, dto.ThirdBreakDurationInMinutes ?? 0));
        }

        breaks.Sort((a, b) => TimeSpan.Parse(a.StartTime).CompareTo(TimeSpan.Parse(b.StartTime)));

        return breaks;
    }


    private static List<PeriodDto> BuildPeriods(SchoolDto schoolDto, ILogger logger)
    {
        var periods = new List<PeriodDto>();

        var breaks = BuildBreaks(schoolDto);

        // start time of current period, initialized to class start time
        var periodStartTime = schoolDto.ClassStartTime.AsTimeSpan();

        // end time of current period, initialized to start time + class duration (will be adjusted if breaks occur)
        var periodEndTime = periodStartTime.Add(TimeSpan.FromMinutes(schoolDto.ClassDurationInMinutes));
        var periodDuration = schoolDto.ClassDurationInMinutes;

        var currentTime = periodStartTime;
        var nextBreakIndex = 0;
        var nextBreak = breaks.First();

        try
        {
            while (periodEndTime.CompareTo(TimeSpan.Parse(schoolDto.ClassEndTime)) <= 0)
            {
                if (nextBreak != null && currentTime <= TimeSpan.Parse(nextBreak.StartTime) && periodEndTime > TimeSpan.Parse(nextBreak.StartTime))
                {
                    // If current time has reached the next break, add the break period
                    var breakEndTime = TimeSpan.Parse(nextBreak.StartTime).Add(TimeSpan.FromMinutes(nextBreak.Duration));
                    var breakPeriodIndex = periods.Count + 1;
                    var breakPeriod = new PeriodDto
                    (
                        BREAK,
                        1,
                        nextBreak.StartTime,
                        breakEndTime.AsString(),
                        null,
                        null,
                        Guid.NewGuid().ToString()
                    );

                    periods.Add(breakPeriod);
                    logger.LogInformation($"Added break period: {breakPeriod.StartTime} - {breakPeriod.EndTime}");
                    // Move current time to end of break and update next break
                    currentTime = breakEndTime;
                    nextBreakIndex++;
                    nextBreak = nextBreakIndex < breaks.Count ? breaks[nextBreakIndex] : null;
                    periodEndTime = currentTime.Add(TimeSpan.FromMinutes(periodDuration));
                }
                else
                {
                    // Add a course period
                    var coursePeriodIndex = periods.Count + 1;
                    var courseEndTime = currentTime.Add(TimeSpan.FromMinutes(periodDuration));
                    var coursePeriod = new PeriodDto
                    (
                        COURSE,
                        1,
                        currentTime.AsString(),
                        courseEndTime.AsString(),
                        null,
                        null,
                        Guid.NewGuid().ToString()
                    );

                    periods.Add(coursePeriod);
                    logger.LogInformation($"Added course period: {coursePeriod.StartTime} - {coursePeriod.EndTime}");
                    currentTime = courseEndTime;
                    periodEndTime = currentTime.Add(TimeSpan.FromMinutes(periodDuration));
                }
            }
            if (IsPeriodInvalid(periods, schoolDto))
            {
                logger.LogError($"Generated periods are invalid for school: {schoolDto.SchoolId}");
                LogObjects(periods, logger);
                throw new Exception($"Generated periods are invalid for school: {schoolDto.SchoolId}");
            }
            return periods;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error building periods: {ex.Message}");
        }
    }

    private static void LogObjects<T>(List<T> entities, ILogger logger)
    {
        string json = JsonSerializer.Serialize(entities, new JsonSerializerOptions
        {
            WriteIndented = true // pretty-print
        });

        logger.LogInformation("Entities: {Entities}", json);
    }

    public static List<PeriodDto> generatePeriods(SchoolDto dto, ILogger logger)
    {
        logger.LogInformation($"Generating periods for school: {dto.Name}");
        try
        {
            var periods = BuildPeriods(dto, logger);
            var result = new List<PeriodDto>();

            var Days = new[] { 1, 2, 3, 4, 5 };

            foreach (var day in Days)
            {
                if (day == 1) { continue; }
                foreach (var period in periods)
                {
                    var copy = period with { Day = day, PeriodId = Guid.NewGuid().ToString() };
                    result.Add(copy);
                }
            }
            logger.LogInformation($"Finished generating periods for school: {dto.SchoolId}");
            periods.AddRange(result);
            return periods;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error generating periods for school: {dto.SchoolId} - {ex.Message}");
            throw new Exception($"Error generating periods for school: {ex.Message}");
        }

    }


    private static bool IsPeriodInvalid(List<PeriodDto> periods, SchoolDto school)
    {
        var periodsDurationInMinute = periods.Sum(p => TimeSpan.Parse(p.EndTime).Subtract(TimeSpan.Parse(p.StartTime)).TotalMinutes);
        var schoolDurationInMinute = TimeSpan.Parse(school.ClassEndTime).Subtract(TimeSpan.Parse(school.ClassStartTime)).TotalMinutes;
        return periodsDurationInMinute != schoolDurationInMinute;
    }
}
