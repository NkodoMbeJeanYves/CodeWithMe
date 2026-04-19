using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMe.Middlewares;

internal sealed record ValidationReport(string propertyName, string errorMessage);

public class ValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider services)
    {
        // Récupérer l'endpoint et ses arguments
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var actionDescriptor = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
        if (actionDescriptor == null)
        {
            await _next(context);
            return;
        }

        // Parcourir les paramètres de l'action
        foreach (var parameter in actionDescriptor.Parameters)
        {
            var paramType = parameter.ParameterType;

            // Vérifier si un validator existe pour ce type
            var validatorType = typeof(IValidator<>).MakeGenericType(paramType);
            var validator = services.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                // Lire le body et désérialiser
                context.Request.EnableBuffering();
                var dto = await System.Text.Json.JsonSerializer.DeserializeAsync(
                    context.Request.Body,
                    paramType,
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                context.Request.Body.Position = 0;

                if (dto != null)
                {
                    var validationResult = validator.Validate(new ValidationContext<object>(dto));
                    if (!validationResult.IsValid)
                    {
                        var route = context.Request.Path;
                        var method = context.Request.Method;

                        var problem = new ValidationProblemDetails()
                        {
                            Errors = validationResult.Errors
                                .Select(it => new ValidationReport(it.PropertyName, it.ErrorMessage))
                                .ToDictionary(item => item.propertyName, item => new string[] { item.errorMessage }),
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Validation Error",
                            Type = $"Route: {route}, Method: {method}"
                        };

                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(problem);
                        return; // stop pipeline
                    }
                }
            }
        }

        // Continuer si pas d'erreur
        await _next(context);
    }
}
