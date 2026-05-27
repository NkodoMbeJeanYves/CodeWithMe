namespace CodeWithMe.Core.Models.Enums;

/// <summary>
/// Niveau d'autorité dérivé du <see cref="Role"/> (cf. contrat section 7.3).
/// Calculé côté backend, jamais stocké.
/// </summary>
public enum AuthorityLevel
{
    Institutionnel,
    Pedagogique,
    Support,
    Externe
}
