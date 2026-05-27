using System.Diagnostics;
using CodeWithMe.Core.Models.Envelope;
using CodeWithMe.Core.Services;

namespace CodeWithMe.Middlewares;

/// <summary>
/// Résout le tenant courant à partir du header <c>X-Tenant-Id</c> et du claim <c>tenantId</c>
/// du JWT. Hydrate <see cref="ITenantContext"/> pour le filtre global d'EF.
/// Exempte les routes publiques (/v1/auth/*, /v1/health, /openapi/*).
/// Contrat EDU Platform v1, section 3.
/// </summary>
public sealed class TenantResolutionMiddleware
{
    private const string HeaderName = "X-Tenant-Id";
    private const string TenantClaim = "tenantId";
    private const string RoleClaim = "role";

    private static readonly string[] PublicPathPrefixes =
    {
        "/v1/auth/",
        "/v1/health",
        "/v1/openapi",
        "/openapi/",
        "/api-docs",
        "/swagger",
        "/scalar"
    };

    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
    {
        var path = context.Request.Path.ToString();
        if (IsPublic(path) || !context.User.Identity?.IsAuthenticated == true)
        {
            await _next(context);
            return;
        }

        var jwtTenant = context.User.FindFirst(TenantClaim)?.Value;
        var headerTenant = context.Request.Headers[HeaderName].ToString();
        if (string.IsNullOrEmpty(headerTenant))
            headerTenant = jwtTenant;

        if (string.IsNullOrEmpty(headerTenant) && string.IsNullOrEmpty(jwtTenant))
        {
            await WriteError(context, 400, "VALIDATION_FAILED", $"Header '{HeaderName}' is required.");
            return;
        }

        var role = context.User.FindFirst(RoleClaim)?.Value;
        var isSuperAdmin = string.Equals(role, "super_admin", StringComparison.OrdinalIgnoreCase)
                          || string.Equals(role, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        if (!isSuperAdmin
            && !string.IsNullOrEmpty(jwtTenant)
            && !string.IsNullOrEmpty(headerTenant)
            && !string.Equals(jwtTenant, headerTenant, StringComparison.OrdinalIgnoreCase))
        {
            await WriteError(context, 403, "TENANT_MISMATCH",
                "Tenant header does not match the tenant from your JWT.");
            return;
        }

        tenantContext.Set(headerTenant!);
        await _next(context);
    }

    private static bool IsPublic(string path)
    {
        foreach (var prefix in PublicPathPrefixes)
        {
            if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    private static async Task WriteError(HttpContext ctx, int status, string code, string message)
    {
        var payload = new ApiErrorResponse(
            new[] { new ApiError(code, message) },
            Activity.Current?.TraceId.ToString() ?? ctx.TraceIdentifier,
            DateTime.UtcNow,
            ctx.Request.Path
        );
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(payload);
    }
}
