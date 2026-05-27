using FluentValidation;

namespace CodeWithMe.Core.Dtos.Subject;

public class SubjectDtoValidator : AbstractValidator<SubjectDto>
{
    public SubjectDtoValidator()
    {
        RuleFor(dto => dto.SubjectName)
            .NotEmpty().WithMessage("SubjectName Field is mandatory");
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description Field is mandatory");
    }
}
