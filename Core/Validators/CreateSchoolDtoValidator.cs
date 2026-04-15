using CodeWithMe.Core.Dtos.School;
using FluentValidation;
using System.Globalization;

namespace CodeWithMe.Core.Validators
{
    public class CreateSchoolDtoValidator : AbstractValidator<CreateSchoolDto>
    {
        public CreateSchoolDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name Field is mandatory");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Description Field is mandatory");

            RuleFor(dto => dto.SchoolType)
                .NotEmpty().WithMessage("SchoolType Field is mandatory")
                .IsInEnum();

            RuleFor(dto => dto.ClassStartTime)
                .Must(BeAValidDate).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.ClassEndTime)
                .Must(BeAValidDate).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.ClassDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakDurationInMinutes)
                .GreaterThan(0).WithMessage("This Field must be greater than 0");

            RuleFor(dto => dto.FirstBreakStartTime)
                .Must(BeAValidDate).WithMessage("Date must be in yyyy-MM-dd format");

            RuleFor(dto => dto.SecondBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.SecondBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.SecondBreakStartTime)
                .Must(BeAValidDate).When(dto => !string.IsNullOrEmpty(dto.SecondBreakStartTime)).WithMessage("Date must be in yyyy-MM-dd format if provided");

            RuleFor(dto => dto.ThirdBreakDurationInMinutes)
                .GreaterThan(0).When(dto => dto.ThirdBreakDurationInMinutes.HasValue).WithMessage("This Field must be greater than 0 if provided");

            RuleFor(dto => dto.ThirdBreakStartTime)
                .Must(BeAValidDate).When(dto => !string.IsNullOrEmpty(dto.ThirdBreakStartTime)).WithMessage("Date must be in yyyy-MM-dd format if provided");


        }

        private bool BeAValidDate(string value)
        {
            return DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }
    }
}
