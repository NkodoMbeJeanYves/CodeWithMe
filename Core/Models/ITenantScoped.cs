namespace CodeWithMe.Core.Models;

/// <summary>
/// Implémenté par toute entité métier soumise au cloisonnement multi-tenant.
/// Le filtre global d'<see cref="CodeWithMe.Core.ApiContext"/> n'expose à une requête
/// que les lignes dont <see cref="TenantId"/> correspond au tenant courant.
/// </summary>
public interface ITenantScoped
{
    string TenantId { get; set; }
}
