using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("audit_logs")]
public class AuditLog : ITenantScoped
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
    public string? UserId { get; set; }

    [Column("action")]
    [MaxLength(32)]
    public required string Action { get; set; }

    [Column("resource_type")]
    [MaxLength(64)]
    public required string ResourceType { get; set; }

    [Column("resource_id")]
    [MaxLength(64)]
    public string? ResourceId { get; set; }

    [Column("changes", TypeName = "json")]
    public string? ChangesJson { get; set; }

    [Column("ip_address")]
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    [Column("occurred_at")]
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
