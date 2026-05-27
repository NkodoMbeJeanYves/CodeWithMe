using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("messages")]
public class Message : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("from_user_id")]
    [MaxLength(450)]
    public required string FromUserId { get; set; }

    [Column("to_user_id")]
    [MaxLength(450)]
    public required string ToUserId { get; set; }

    [Column("subject")]
    [MaxLength(255)]
    public string? Subject { get; set; }

    [Column("body")]
    [MaxLength(8000)]
    public required string Body { get; set; }

    [Column("read_at")]
    public DateTime? ReadAt { get; set; }
}
