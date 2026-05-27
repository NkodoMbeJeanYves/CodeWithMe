using System.Diagnostics;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Envelope;
using Microsoft.AspNetCore.Diagnostics;

namespace CodeWithMe.Middlewares;

/// <summary>
/// Convertit toute exception non gérée en payload <see cref="ApiErrorResponse"/>
/// conforme au contrat EDU Platform v1 (section 6 — modèle d'erreur standardisé).
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, errors) = MapException(exception);

        if (status >= 500)
        {
            _logger.LogError(exception, "Unhandled exception on {Path}", httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception, "Domain exception {Code} on {Path}", errors[0].Code, httpContext.Request.Path);
        }

        var payload = new ApiErrorResponse(
            errors,
            ResolveTraceId(httpContext),
            DateTime.UtcNow,
            httpContext.Request.Path
        );

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken: cancellationToken);
        return true;
    }

    private static (int Status, IReadOnlyList<ApiError> Errors) MapException(Exception exception)
        => exception switch
        {
            ValidationException ve
                => (ve.StatusCode, ve.ValidationErrors),

            DomainException dx
                => (dx.StatusCode, new[] { new ApiError(dx.Code, dx.Message, dx.Field, dx.Details) }),

            UnauthorizedAccessException
                => (403, new[] { new ApiError("FORBIDDEN", "Access denied.") }),

            BadHttpRequestException bex
                => (400, new[] { new ApiError("INVALID_QUERY", bex.Message) }),

            _ => (500, new[] { new ApiError("INTERNAL_ERROR", "An unexpected error occurred.") })
        };

    private static string ResolveTraceId(HttpContext ctx)
        => Activity.Current?.TraceId.ToString() ?? ctx.TraceIdentifier;
}
