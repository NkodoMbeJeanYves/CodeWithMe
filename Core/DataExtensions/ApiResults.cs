using CodeWithMe.Core.Models.Envelope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMe.Core.DataExtensions;

/// <summary>
/// Helpers de construction de réponses HTTP alignées au contrat EDU Platform v1
/// (enveloppe data/meta/links pour les succès, errors[] RFC 7807 pour les erreurs).
/// </summary>
public static class ApiResults
{
    public static OkObjectResult Ok<T>(T data) => new(new ApiEnvelope<T>(data));

    public static CreatedResult Created<T>(string location, T data)
        => new(location, new ApiEnvelope<T>(data));

    public static NoContentResult NoContent() => new();

    public static OkObjectResult Page<T>(IReadOnlyList<T> items, PageMeta meta, HttpRequest request)
    {
        var links = BuildLinks(request, meta);
        return new OkObjectResult(new ApiEnvelope<IReadOnlyList<T>>(items, meta, links));
    }

    public static ObjectResult Error(
        int statusCode,
        string code,
        string message,
        HttpContext httpContext,
        string? field = null,
        object? details = null)
    {
        var payload = new ApiErrorResponse(
            new[] { new ApiError(code, message, field, details) },
            ResolveTraceId(httpContext),
            DateTime.UtcNow,
            httpContext.Request.Path
        );
        return new ObjectResult(payload) { StatusCode = statusCode };
    }

    public static ObjectResult Errors(
        int statusCode,
        IReadOnlyList<ApiError> errors,
        HttpContext httpContext)
    {
        var payload = new ApiErrorResponse(
            errors,
            ResolveTraceId(httpContext),
            DateTime.UtcNow,
            httpContext.Request.Path
        );
        return new ObjectResult(payload) { StatusCode = statusCode };
    }

    private static string ResolveTraceId(HttpContext ctx)
        => System.Diagnostics.Activity.Current?.TraceId.ToString() ?? ctx.TraceIdentifier;

    private static PageLinks BuildLinks(HttpRequest request, PageMeta meta)
    {
        var basePath = request.Path.ToString();

        string Build(int page)
        {
            var dict = request.Query.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
            dict["page"] = page.ToString();
            dict["size"] = meta.Size.ToString();
            var qs = string.Join("&", dict.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));
            return string.IsNullOrEmpty(qs) ? basePath : $"{basePath}?{qs}";
        }

        var totalPages = Math.Max(meta.TotalPages, 1);
        return new PageLinks(
            Self: Build(meta.Page),
            Next: meta.Page < totalPages ? Build(meta.Page + 1) : null,
            Prev: meta.Page > 1 ? Build(meta.Page - 1) : null,
            First: Build(1),
            Last: Build(totalPages)
        );
    }
}
