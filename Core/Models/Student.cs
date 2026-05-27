using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("students")]
public class Student : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("user_id")]
    [MaxLength(450)] // IdentityUser.Id default length
    public required string UserId { get; set; }

    [Column("matricule")]
    [MaxLength(16)]
    public required string Matricule { get; set; }

    [Column("classe_id")]
    [MaxLength(36)]
    public required string ClasseId { get; set; }

    [Column("filiere_id")]
    [MaxLength(36)]
    public string? FiliereId { get; set; }

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("gender")]
    public Gender? Gender { get; set; }

    [Column("parent_ids", TypeName = "json")]
    public string? ParentIdsJson { get; set; }

    [Column("enrolled_at")]
    public DateTime? EnrolledAt { get; set; }

    [Column("status")]
    public StudentStatus Status { get; set; } = StudentStatus.Enrolled;

    [Column("contact", TypeName = "json")]
    public string? ContactJson { get; set; }
}
