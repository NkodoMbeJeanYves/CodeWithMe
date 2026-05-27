using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("rooms")]
public class Room : HasTimestamps, ITenantScoped
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

    [Column("capacity")]
    public int? Capacity { get; set; }
}
