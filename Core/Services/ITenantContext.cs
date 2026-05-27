namespace CodeWithMe.Core.Services;

/// <summary>
/// Tenant courant pour le scope de la requête. Hydraté par <see cref="Middlewares.TenantResolutionMiddleware"/>
/// puis lu par le filtre global EF de <see cref="Core.ApiContext"/>.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// Id du tenant courant, ou <c>null</c> tant que le middleware n'a pas été exécuté
    /// (cas des endpoints publics : /v1/auth/*, /v1/health, /openapi/v1.json).
    /// </summary>
    string? CurrentTenantId { get; }

    void Set(string tenantId);
}

public sealed class TenantContext : ITenantContext
{
    public string? CurrentTenantId { get; private set; }

    public void Set(string tenantId)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
            throw new ArgumentException("tenantId required", nameof(tenantId));
        CurrentTenantId = tenantId;
    }
}
