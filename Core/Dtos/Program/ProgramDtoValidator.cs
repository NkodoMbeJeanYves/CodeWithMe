using FluentValidation;

namespace CodeWithMe.Core.Dtos.Program;

public class ProgramDtoValidator : AbstractValidator<ProgramDto>
{
    public ProgramDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
        RuleFor(dto => dto.SchoolId)
            .NotEmpty()
            .WithMessage("SchoolId is required.");
    }
}
