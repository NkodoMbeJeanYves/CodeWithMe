using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("timetables")]
public class Timetable : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("owner_type")]
    public TimetableOwnerType OwnerType { get; set; }

    [Column("owner_id")]
    [MaxLength(36)]
    public required string OwnerId { get; set; }

    [Column("week_of")]
    public DateTime WeekOf { get; set; }

    public ICollection<TimetableEntry> Entries { get; set; } = new List<TimetableEntry>();
}
