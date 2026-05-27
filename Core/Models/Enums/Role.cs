namespace CodeWithMe.Core.Models.Enums;

/// <summary>
/// Les 12 rôles RBAC du contrat EDU Platform v1 (section 2.2 et 7.3).
/// La sérialisation JSON convertit en snake_case (super_admin, directeur_pedagogique, ...).
/// </summary>
public enum Role
{
    SuperAdmin,
    Directeur,
    DirecteurPedagogique,
    ResponsableAdministratif,
    Enseignant,
    Surveillant,
    Secretaire,
    Comptable,
    Bibliothecaire,
    Apprenant,
    Parent,
    Externe
}
