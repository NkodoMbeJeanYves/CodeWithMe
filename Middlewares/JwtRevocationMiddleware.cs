using CodeWithMe.Core;
using System.Security.Claims;

namespace CodeWithMe.Middlewares;

public class JwtRevocationMiddleware
{
    private readonly RequestDelegate _next;

    public JwtRevocationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApiContext db)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var identifier = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(identifier))
            {
                var revoked = db.RevokedTokens.Any(rt => rt.Jti == identifier);
                if (revoked)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been revoked");
                    return;
                }
            }
        }

        await _next(context);
    }
}

