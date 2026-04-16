using FluentValidation;

namespace CodeWithMe.Core.Validators
{
    public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
    {
        public WeatherForecastValidator()
        {
            // Date obligatoire et non par défaut
            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            // Température obligatoire et dans une plage réaliste
            RuleFor(x => x.TemperatureC)
                .NotEmpty().WithMessage("TempC is required.")
                .InclusiveBetween(-50, 60).WithMessage("TemperatureC must be between -50 and 60 °C.");

            // Summary obligatoire et avec une longueur minimale
            RuleFor(x => x.Summary)
                .NotEmpty().WithMessage("Summary is required.")
                .MinimumLength(3).WithMessage("Summary must be at least 3 characters long.");
        }
    }
}
