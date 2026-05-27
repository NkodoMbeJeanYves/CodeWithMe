using System.Diagnostics;
using CodeWithMe.Core.Models.Envelope;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CodeWithMe.Middlewares;

/// <summary>
/// Exécute la validation FluentValidation sur les body params des actions controller
/// et émet une réponse 400 alignée au contrat (errors[] avec code VALIDATION_FAILED).
/// </summary>
public class ValidationMiddleware
{
    private static readonly System.Text.Json.JsonSerializerOptions JsonOpts =
        new() { PropertyNameCaseInsensitive = true };

    private readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider services)
    {
        var endpoint = context.GetEndpoint();
        var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (actionDescriptor is null)
        {
            await _next(context);
            return;
        }

        foreach (var parameter in actionDescriptor.Parameters)
        {
            var paramType = parameter.ParameterType;
            var validatorType = typeof(IValidator<>).MakeGenericType(paramType);
            if (services.GetService(validatorType) is not IValidator validator)
                continue;

            context.Request.EnableBuffering();
            if (context.Request.ContentLength is null or 0)
            {
                context.Request.Body.Position = 0;
                continue;
            }

            object? dto;
            try
            {
                dto = await System.Text.Json.JsonSerializer.DeserializeAsync(
                    context.Request.Body, paramType, JsonOpts);
            }
            catch (System.Text.Json.JsonException jex)
            {
                await WriteErrors(context, 400, new[]
                {
                    new ApiError("INVALID_QUERY", $"Malformed JSON body: {jex.Message}")
                });
                return;
            }
            finally
            {
                context.Request.Body.Position = 0;
            }

            if (dto is null)
                continue;

            var result = await validator.ValidateAsync(new ValidationContext<object>(dto));
            if (result.IsValid)
                continue;

            var errors = result.Errors
                .Select(f => new ApiError(
                    Code: "VALIDATION_FAILED",
                    Message: f.ErrorMessage,
                    Field: ToCamelCase(f.PropertyName)))
                .ToArray();

            await WriteErrors(context, 400, errors);
            return;
        }

        await _next(context);
    }

    private static async Task WriteErrors(HttpContext ctx, int status, IReadOnlyList<ApiError> errors)
    {
        var payload = new ApiErrorResponse(
            errors,
            Activity.Current?.TraceId.ToString() ?? ctx.TraceIdentifier,
            DateTime.UtcNow,
            ctx.Request.Path
        );

        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(payload);
    }

    private static string ToCamelCase(string property)
    {
        if (string.IsNullOrEmpty(property)) return property;
        // FluentValidation renvoie "Address.City" — on traite chaque segment.
        return string.Join('.', property.Split('.')
            .Select(seg => char.IsUpper(seg[0]) ? char.ToLowerInvariant(seg[0]) + seg[1..] : seg));
    }
}
