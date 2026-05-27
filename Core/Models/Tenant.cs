using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

/// <summary>
/// Établissement (ou tenant) du contrat EDU Platform v1, section 2.1.
/// Identifiant primaire = uuid v4 stocké en string.
/// </summary>
[Table("tenants")]
public class Tenant : HasTimestamps
{
    public const string DefaultTenantId = "00000000-0000-4000-8000-000000000001";
    public const string DefaultTenantCode = "DEFAULT";

    [Key]
    [Column("id")]
    public required string Id { get; set; }

    [Column("code")]
    [MaxLength(16)]
    public required string Code { get; set; }

    [Column("name")]
    [MaxLength(120)]
    public required string Name { get; set; }

    [Column("type")]
    public TenantType Type { get; set; } = TenantType.School;

    [Column("status")]
    public TenantStatus Status { get; set; } = TenantStatus.Active;

    [Column("locale")]
    [MaxLength(8)]
    public string Locale { get; set; } = "fr-FR";

    [Column("timezone")]
    [MaxLength(64)]
    public string Timezone { get; set; } = "Europe/Paris";

    [Column("academic_year")]
    [MaxLength(9)]
    public string? AcademicYear { get; set; }

    /// <summary>
    /// Paramètres tenant (gradingScale, weekStartsOn, ...) sérialisés en JSON.
    /// </summary>
    [Column("settings", TypeName = "json")]
    public string? SettingsJson { get; set; }
}
