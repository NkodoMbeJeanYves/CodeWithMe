using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("notifications")]
public class Notification : HasTimestamps, ITenantScoped
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

    [Column("type")]
    [MaxLength(64)]
    public required string Type { get; set; }

    [Column("title")]
    [MaxLength(255)]
    public required string Title { get; set; }

    [Column("body")]
    [MaxLength(2000)]
    public string? Body { get; set; }

    [Column("read_at")]
    public DateTime? ReadAt { get; set; }

    [Column("data", TypeName = "json")]
    public string? DataJson { get; set; }
}
