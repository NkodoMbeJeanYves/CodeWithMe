using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Security;

/// <summary>
/// Mapping Rôle → Niveau d'autorité, d'après le contrat EDU Platform v1 (section 7.3).
/// </summary>
public static class AuthorityMap
{
    public static AuthorityLevel For(Role role) => role switch
    {
        Role.SuperAdmin
            or Role.Directeur
            or Role.DirecteurPedagogique
            or Role.ResponsableAdministratif => AuthorityLevel.Institutionnel,

        Role.Enseignant => AuthorityLevel.Pedagogique,

        Role.Surveillant
            or Role.Secretaire
            or Role.Comptable
            or Role.Bibliothecaire => AuthorityLevel.Support,

        Role.Apprenant or Role.Parent or Role.Externe => AuthorityLevel.Externe,

        _ => AuthorityLevel.Externe
    };
}
