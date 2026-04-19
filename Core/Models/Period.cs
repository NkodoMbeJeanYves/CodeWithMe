//using ITimestamps = CodeWithMe.Core.IHasTimestamps;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("periods")]
public class Period : IHasTimestamps
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

    [Column("school_id")]
    public required string? SchoolId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

}
