using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("attendances")]
public class Attendance : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("session_id")]
    [MaxLength(36)]
    public required string SessionId { get; set; }

    [Column("student_id")]
    [MaxLength(36)]
    public required string StudentId { get; set; }

    [Column("status")]
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    [Column("minutes_late")]
    public int? MinutesLate { get; set; }

    [Column("justification")]
    [MaxLength(1000)]
    public string? Justification { get; set; }

    [Column("document_id")]
    [MaxLength(36)]
    public string? DocumentId { get; set; }
}
