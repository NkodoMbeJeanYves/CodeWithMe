using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMe.Core.Models;

/// <summary>
/// User étendu pour le contrat EDU Platform v1.
/// Aligne section 2.2 (rôle, niveau d'autorité, rattachement filière/classe) et 7.3 (mapping autorité).
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Tenant de rattachement. Requis sauf pour <see cref="Enums.Role.SuperAdmin"/>.
    /// </summary>
    [Column("tenant_id")]
    [MaxLength(36)]
    public string? TenantId { get; set; }

    [Column("role")]
    public Role Role { get; set; } = Role.Externe;

    [Column("first_name")]
    [MaxLength(60)]
    public string? FirstName { get; set; }

    [Column("last_name")]
    [MaxLength(60)]
    public string? LastName { get; set; }

    [Column("avatar_url")]
    [MaxLength(2048)]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Liste d'IDs filières (UUID v4) — JSON serialisé. Réservé V1, pas enforced.
    /// </summary>
    [Column("filiere_ids", TypeName = "json")]
    public string? FiliereIdsJson { get; set; }

    /// <summary>
    /// Liste d'IDs classes (UUID v4) — JSON serialisé.
    /// </summary>
    [Column("classe_ids", TypeName = "json")]
    public string? ClasseIdsJson { get; set; }

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }
}
