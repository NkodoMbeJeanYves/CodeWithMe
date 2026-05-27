using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("teachers")]
public class Teacher : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("user_id")]
    [MaxLength(450)]
    public required string UserId { get; set; }

    [Column("matricule")]
    [MaxLength(16)]
    public required string Matricule { get; set; }

    [Column("subject_ids", TypeName = "json")]
    public string? SubjectIdsJson { get; set; }

    [Column("classe_ids", TypeName = "json")]
    public string? ClasseIdsJson { get; set; }

    [Column("hire_date")]
    public DateTime? HireDate { get; set; }

    [Column("contract_type")]
    public ContractType ContractType { get; set; } = ContractType.Permanent;

    [Column("weekly_hours")]
    public int WeeklyHours { get; set; }

    [Column("status")]
    public TeacherStatus Status { get; set; } = TeacherStatus.Active;
}
