using FluentValidation;

namespace CodeWithMe.Core.Dtos.Period;

public enum PeriodTypes
{
    COURSE = 1,
    BREAK = 2
}

public class PeriodDtoValidator : AbstractValidator<PeriodDto>
{
    private static readonly string[] AllowedTypes =
            { "COURSE", "BREAK" };
    public PeriodDtoValidator()
    {
        RuleFor(dto => dto.Type)
           .Must(value => AllowedTypes.Contains(value))
           .WithMessage($"PeriodType must be one of: {string.Join(", ", AllowedTypes)}");

        RuleFor(dto => dto.Day)
            .InclusiveBetween(1, 6)
            .WithMessage("Day must be between 1 and 6.");

        RuleFor(dto => dto).Custom((dto, context) =>
        {
            if (!TimeSpan.TryParseExact(dto.EndTime, "hh\\:mm", null, out var endTime))
            {
                context.AddFailure("ClassEndTime", "Invalid format, expected HH:mm");
            }

            if (!TimeSpan.TryParseExact(dto.StartTime, "hh\\:mm", null, out var startTime))
            {
                context.AddFailure("ClassStartTime", "Invalid format, expected HH:mm");
            }

            if (endTime <= startTime)
            {
                context.AddFailure("_", "End time must be after start time");
            }
        });

    }

}
