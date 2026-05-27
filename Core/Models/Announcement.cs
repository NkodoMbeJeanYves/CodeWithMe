using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models;

[Table("announcements")]
public class Announcement : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("title")]
    [MaxLength(255)]
    public required string Title { get; set; }

    [Column("body")]
    [MaxLength(8000)]
    public required string Body { get; set; }

    [Column("audience", TypeName = "json")]
    public string? AudienceJson { get; set; }

    [Column("published_by")]
    [MaxLength(450)]
    public string? PublishedBy { get; set; }

    [Column("published_at")]
    public DateTime? PublishedAt { get; set; }

    [Column("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}
