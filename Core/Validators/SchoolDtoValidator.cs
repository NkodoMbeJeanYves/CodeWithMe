using CodeWithMe.Core.Dtos.School;
using FluentValidation;

namespace CodeWithMe.Core.Validators
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
                .IsInEnum();

            RuleFor(dto => dto.ClassStartTime)
                .Must(BeAValidTime).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.ClassEndTime)
                .Must(BeAValidTime).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.ClassDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakStartTime)
                .Must(BeAValidTime).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.SecondBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.SecondBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.SecondBreakStartTime)
                .Must(value => value >= TimeSpan.Zero && value < TimeSpan.FromDays(1)).WithMessage("StartTime must be a valid time of day (00:00 to 23:59).");

            RuleFor(dto => dto.ThirdBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.ThirdBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.ThirdBreakStartTime)
                .Must(value => value >= TimeSpan.Zero && value < TimeSpan.FromDays(1)).WithMessage("StartTime must be a valid time of day (00:00 to 23:59).");


        }

        private bool BeAValidTime(TimeSpan value)
        {
            return value >= TimeSpan.Zero && value < TimeSpan.FromDays(1);
        }
    }
}
