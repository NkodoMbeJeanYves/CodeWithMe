using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("course_sessions")]
public class CourseSession : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("subject_id")]
    [MaxLength(36)]
    public required string SubjectId { get; set; }

    [Column("teacher_id")]
    [MaxLength(36)]
    public required string TeacherId { get; set; }

    [Column("classe_id")]
    [MaxLength(36)]
    public required string ClasseId { get; set; }

    [Column("room_id")]
    [MaxLength(36)]
    public string? RoomId { get; set; }

    [Column("start_at")]
    public DateTime StartAt { get; set; }

    [Column("end_at")]
    public DateTime EndAt { get; set; }

    [Column("type")]
    public SessionType Type { get; set; } = SessionType.Lecture;

    [Column("status")]
    public SessionStatus Status { get; set; } = SessionStatus.Scheduled;

    [Column("attendance_recorded")]
    public bool AttendanceRecorded { get; set; }

    [Column("present_count")]
    public int PresentCount { get; set; }

    [Column("absent_count")]
    public int AbsentCount { get; set; }

    [Column("notes")]
    [MaxLength(2000)]
    public string? Notes { get; set; }

    [Column("cancel_reason")]
    [MaxLength(255)]
    public string? CancelReason { get; set; }
}
