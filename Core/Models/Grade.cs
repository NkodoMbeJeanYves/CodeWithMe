using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("grades")]
public class Grade : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("student_id")]
    [MaxLength(36)]
    public required string StudentId { get; set; }

    [Column("subject_id")]
    [MaxLength(36)]
    public required string SubjectId { get; set; }

    [Column("teacher_id")]
    [MaxLength(36)]
    public required string TeacherId { get; set; }

    [Column("session_id")]
    [MaxLength(36)]
    public string? SessionId { get; set; }

    [Column("value")]
    public double Value { get; set; }

    [Column("scale")]
    public double Scale { get; set; } = 20;

    [Column("coefficient")]
    public double Coefficient { get; set; } = 1;

    [Column("type")]
    public GradeType Type { get; set; } = GradeType.Quiz;

    [Column("period")]
    public GradePeriod Period { get; set; } = GradePeriod.T1;

    [Column("comment")]
    [MaxLength(500)]
    public string? Comment { get; set; }

    [Column("validated_by")]
    [MaxLength(450)]
    public string? ValidatedBy { get; set; }

    [Column("published_at")]
    public DateTime? PublishedAt { get; set; }
}
