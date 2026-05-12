using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("periods")]
public class Period : HasTimestamps
{
    [Column("period_id")]
    [Key()]
    public required string PeriodId { get; set; }

    [Column("day")]
    public required int Day { get; set; }

    [Column("period_type")]
    public required string PeriodType { get; set; }

    [Column("start_time")]
    public required TimeSpan StartTime { get; set; }

    [Column("end_time")]
    public required TimeSpan EndTime { get; set; }

    [Column("event_id")]
    public required string? EventId { get; set; }

    // Relationship
    [Column("school_id")]
    [ForeignKey("school")]
    public required string? SchoolId { get; set; }
    //public School? School { get; set; } = null;

}
