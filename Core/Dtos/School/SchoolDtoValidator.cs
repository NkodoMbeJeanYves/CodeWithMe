using FluentValidation;

namespace CodeWithMe.Core.Dtos.School
{
    public class SchoolUpdateDtoValidator : AbstractValidator<SchoolUpdateDto>
    {
        private static readonly string[] AllowedTypes =
            { "COLLEGE", "HIGH SCHOOL", "UNIVERSITY" };
        public SchoolUpdateDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name Field is mandatory");
            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description Field is mandatory");
            RuleFor(dto => dto.SchoolType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("SchoolType Field is mandatory")
                .Must(value => AllowedTypes.Contains(value))
                .WithMessage($"SchoolType must be one of: {string.Join(", ", AllowedTypes)}");
        }
    }

    public class SchoolDtoValidator : AbstractValidator<SchoolDto>
    {
        private static readonly string[] AllowedTypes =
            { "COLLEGE", "HIGH SCHOOL", "UNIVERSITY" };

        public SchoolDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name Field is mandatory");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description Field is mandatory");

            RuleFor(dto => dto.SchoolType)
                .NotEmpty().WithMessage("SchoolType Field is mandatory")
                .Must(value => AllowedTypes.Contains(value))
                .WithMessage($"PeriodType must be one of: {string.Join(", ", AllowedTypes)}");

            RuleFor(dto => dto).Custom((dto, context) =>
            {
                if (!TimeSpan.TryParseExact(dto.ClassEndTime, "hh\\:mm", null, out var endTime))
                {
                    context.AddFailure("ClassEndTime", "Invalid format, expected HH:mm");
                }

                if (!TimeSpan.TryParseExact(dto.ClassStartTime, "hh\\:mm", null, out var startTime))
                {
                    context.AddFailure("ClassStartTime", "Invalid format, expected HH:mm");
                }

                if (endTime <= startTime)
                {
                    context.AddFailure("_", "End time must be after start time");
                }
            });

            RuleFor(dto => dto.ClassDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            // FirstBreak Rules
            RuleFor(dto => dto.FirstBreakDurationInMinutes)
                .GreaterThan(0)
                .WithMessage("FirstBreakDurationInMinutes must be greater than 0.");

            RuleFor(dto => dto.FirstBreakStartTime)
                .Must(time => BeValidTime(time))
                .WithMessage("FirstBreakStartTime must be in HH:mm format");

            // SecondBreak Rules
            RuleFor(dto => dto.SecondBreakDurationInMinutes)
                .GreaterThan(0)
                .When(dto => !string.IsNullOrEmpty(dto.SecondBreakStartTime))
                .WithMessage("SecondBreakDurationInMinutes must be greater than 0 if SecondBreakStartTime is provided");

            RuleFor(dto => dto.SecondBreakStartTime)
                .Must(time => string.IsNullOrEmpty(time) || BeValidTime(time))
                .WithMessage("SecondBreakStartTime must be in HH:mm format if provided");

            // ThirdBreak Rules
            RuleFor(dto => dto.ThirdBreakDurationInMinutes)
                .GreaterThan(0)
                .When(dto => !string.IsNullOrEmpty(dto.ThirdBreakStartTime))
                .WithMessage("ThirdBreakDurationInMinutes must be greater than 0 if ThirdBreakStartTime is provided");

            RuleFor(dto => dto.ThirdBreakStartTime)
                .Must(time => string.IsNullOrEmpty(time) || BeValidTime(time))
                .WithMessage("ThirdBreakStartTime must be in HH:mm format if provided");

            // Validation combinée sur l'ordre des pauses
            RuleFor(dto => dto).Custom((dto, context) =>
            {
                if (!BeValidTime(dto.FirstBreakStartTime)) return;

                var first = TimeSpan.ParseExact(dto.FirstBreakStartTime, "hh\\:mm", null);

                if (!string.IsNullOrEmpty(dto.SecondBreakStartTime) && BeValidTime(dto.SecondBreakStartTime))
                {
                    var second = TimeSpan.ParseExact(dto.SecondBreakStartTime, "hh\\:mm", null);
                    if (second <= first)
                        context.AddFailure("SecondBreakStartTime", "Second break must be after first break");

                    if (!string.IsNullOrEmpty(dto.ThirdBreakStartTime) && BeValidTime(dto.ThirdBreakStartTime))
                    {
                        var third = TimeSpan.ParseExact(dto.ThirdBreakStartTime, "hh\\:mm", null);
                        if (third <= second)
                            context.AddFailure("ThirdBreakStartTime", "Third break must be after second break");
                    }
                }
            });


        }

        public bool BeValidTime(string time)
        {
            return TimeSpan.TryParseExact(
                time,
                "hh\\:mm",              // format strict HH:mm
                null,
                out _
            );
        }
    }
}
