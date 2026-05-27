using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Models;

[Table("payroll_periods")]
public class PayrollPeriod : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("year")]
    public int Year { get; set; }

    [Column("month")]
    public int Month { get; set; }

    [Column("status")]
    public PayrollStatus Status { get; set; } = PayrollStatus.Open;
}

[Table("payroll_runs")]
public class PayrollRun : HasTimestamps, ITenantScoped
{
    [Key]
    [Column("id")]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Column("tenant_id")]
    [MaxLength(36)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;

    [Column("period_id")]
    [MaxLength(36)]
    public required string PeriodId { get; set; }

    [Column("run_at")]
    public DateTime RunAt { get; set; } = DateTime.UtcNow;

    [Column("run_by")]
    [MaxLength(450)]
    public string? RunBy { get; set; }

    [Column("items", TypeName = "json")]
    public string? ItemsJson { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }
}
