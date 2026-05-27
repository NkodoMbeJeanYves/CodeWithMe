using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("invoices")]
public class Invoice : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("student_id")]
    [MaxLength(36)]
    public required string StudentId { get; set; }

    [Column("academic_year")]
    [MaxLength(9)]
    public string? AcademicYear { get; set; }

    [Column("items", TypeName = "json")]
    public string? ItemsJson { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("currency")]
    [MaxLength(3)]
    public string Currency { get; set; } = "EUR";

    [Column("status")]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    [Column("due_date")]
    public DateTime? DueDate { get; set; }
}
