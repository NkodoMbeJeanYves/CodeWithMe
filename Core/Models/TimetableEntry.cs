using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("timetable_entries")]
public class TimetableEntry : HasTimestamps
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("timetable_id")]
    [MaxLength(36)]
    public required string TimetableId { get; set; }

    [Column("day_of_week")]
    public int DayOfWeek { get; set; }

    [Column("start_time")]
    public TimeSpan StartTime { get; set; }

    [Column("end_time")]
    public TimeSpan EndTime { get; set; }

    [Column("session_id")]
    [MaxLength(36)]
    public string? SessionId { get; set; }

    [Column("color")]
    [MaxLength(16)]
    public string? Color { get; set; }
}
