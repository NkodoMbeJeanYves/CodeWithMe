using CodeWithMe.Core.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CodeWithMe.Core.Security;

/// <summary>
/// Restreint l'accès à un endpoint à un ensemble de <see cref="Role"/>.
/// S'appuie sur le claim "role" (snake_case) émis par <see cref="Services.TokenService"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class AllowRolesAttribute : AuthorizeAttribute
{
    public AllowRolesAttribute(params Role[] roles)
    {
        var names = roles
            .Select(r => System.Text.Json.JsonNamingPolicy.SnakeCaseLower.ConvertName(r.ToString()));
        Roles = string.Join(',', names);
    }
}

/// <summary>
/// Restreint l'accès à un endpoint à un ensemble de <see cref="AuthorityLevel"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class AllowAuthorityAttribute : AuthorizeAttribute
{
    public AllowAuthorityAttribute(params AuthorityLevel[] levels)
    {
        var policies = levels.Select(l => $"Authority.{l}");
        Policy = string.Join(',', policies);
    }
}
