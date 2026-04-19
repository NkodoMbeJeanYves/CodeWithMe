using CodeWithMe.Core.Models;
using FluentValidation;

namespace CodeWithMe.Core.Dtos.School
{
    public class SchoolDtoValidator : AbstractValidator<SchoolDto>
    {
        public SchoolDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name Field is mandatory");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description Field is mandatory");

            RuleFor(dto => dto.SchoolType)
                .NotEmpty().WithMessage("SchoolType Field is mandatory")
                .Must(value => Enum.TryParse(typeof(SchoolTypes), value, true, out _))
                .WithMessage("SchoolType must be one of: 'COLLEGE','HIGH SCHOOL','UNIVERSITY'");

            RuleFor(dto => dto.ClassStartTime)
                .Must(BeAValidTime).WithMessage("Time must be in HH:mm format");

            RuleFor(dto => dto.ClassEndTime)
                .Must(BeValidTime).WithMessage("Time must be in HH:mm format");

            RuleFor(dto => dto.ClassDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakStartTime)
                .Must(value => TimeSpan.TryParseExact(
                value,
                "hh\\:mm",              // format strict HH:mm
                null,
                out _
            )).WithMessage("Time must be in HH:mm format");

            RuleFor(dto => dto.SecondBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.SecondBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.SecondBreakStartTime)
                .Must(value => value >= TimeSpan.Zero && value < TimeSpan.FromDays(1)).WithMessage("StartTime must be a valid time of day (00:00 to 23:59).");

            RuleFor(dto => dto.ThirdBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.ThirdBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.ThirdBreakStartTime)
                .Must(value => value >= TimeSpan.Zero && value < TimeSpan.FromDays(1)).WithMessage("StartTime must be a valid time of day (00:00 to 23:59).");


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

        private bool BeEndTimeAfterStartTime(string start, string end)
        {
            if (!BeValidTime(start) || !BeValidTime(end))
                return false;

            var startTime = TimeSpan.ParseExact(start, "hh\\:mm", null);
            var endTime = TimeSpan.ParseExact(end, "hh\\:mm", null);

            return endTime > startTime;
        }
    }
}
