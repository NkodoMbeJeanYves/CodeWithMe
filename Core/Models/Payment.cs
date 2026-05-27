using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("payments")]
public class Payment : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("invoice_id")]
    [MaxLength(36)]
    public required string InvoiceId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("method")]
    public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

    [Column("received_at")]
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    [Column("reference_code")]
    [MaxLength(64)]
    public string? ReferenceCode { get; set; }
}
