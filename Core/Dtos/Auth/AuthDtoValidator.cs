using FluentValidation;

namespace CodeWithMe.Core.Dtos.Auth;

/// <summary>
/// Validator for LoginDto using FluentValidation
/// Validates username, password, and email fields
/// </summary>
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
                .WithMessage("Username is required.")
            .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50)
                .WithMessage("Username must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9_-]+$")
                .WithMessage("Username can only contain letters, numbers, underscores, and hyphens.");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128)
                .WithMessage("Password must not exceed 128 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&).");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email is required.")
            .EmailAddress()
                .WithMessage("Email must be a valid email address.")
            .MaximumLength(255)
                .WithMessage("Email must not exceed 255 characters.");
    }
}

/// <summary>
/// Validator for RefreshRequestDto using FluentValidation
/// Validates access token, refresh token, and token expiry
/// </summary>
public class RefreshRequestDtoValidator : AbstractValidator<RefreshRequestDto>
{
    public RefreshRequestDtoValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
                .WithMessage("Access token is required.")
            .MinimumLength(10)
                .WithMessage("Access token appears to be invalid (too short).");

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
                .WithMessage("Refresh token is required.")
            .MinimumLength(10)
                .WithMessage("Refresh token appears to be invalid (too short).");

        RuleFor(x => x.AccessTokenExpiry)
            .NotEmpty()
                .WithMessage("Access token expiry date is required.")
            .GreaterThan(DateTime.UtcNow)
                .WithMessage("Access token expiry date must be in the future.")
            .LessThan(DateTime.UtcNow.AddDays(365))
                .WithMessage("Access token expiry date seems too far in the future.");
    }
}

/// <summary>
/// Validator for RefreshTokenRequestDto using FluentValidation
/// Validates the refresh token field
/// </summary>
public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
                .WithMessage("Token is required.")
            .MinimumLength(10)
                .WithMessage("Token appears to be invalid (too short).");
    }
}
