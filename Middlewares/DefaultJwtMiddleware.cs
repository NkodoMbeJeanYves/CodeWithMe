namespace CodeWithMe.Middlewares;

public class DefaultJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _defaultToken;

    public DefaultJwtMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _defaultToken = config["JwtConfig:DefaultToken"] ?? ""; // token genere et stocké en variable d'environnement
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Request.Headers.Append("Authorization", $"Bearer {_defaultToken}");
        }

        await _next(context);
    }
}

