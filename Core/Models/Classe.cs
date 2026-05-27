using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("classes")]
public class Classe : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("code")]
    [MaxLength(32)]
    public required string Code { get; set; }

    [Column("name")]
    [MaxLength(120)]
    public required string Name { get; set; }

    [Column("level")]
    [MaxLength(32)]
    public string? Level { get; set; }

    [Column("filiere_id")]
    [MaxLength(36)]
    public string? FiliereId { get; set; }

    [Column("academic_year")]
    [MaxLength(9)]
    public string? AcademicYear { get; set; }
}
