using CodeWithMe.Core.Models.Envelope;

namespace CodeWithMe.Core.Exceptions;

/// <summary>
/// Validation FluentValidation regroupée en une exception transportant la liste détaillée des champs en échec.
/// </summary>
public sealed class ValidationException : DomainException
{
    public IReadOnlyList<ApiError> ValidationErrors { get; }

    public ValidationException(IReadOnlyList<ApiError> errors)
        : base("VALIDATION_FAILED", 400, "Validation failed.")
    {
        ValidationErrors = errors;
    }
}
