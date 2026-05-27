using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Finance;

public sealed record InvoiceDto(
    string StudentId,
    string? AcademicYear,
    decimal TotalAmount,
    string Currency,
    InvoiceStatus Status,
    DateTime? DueDate,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record InvoiceCreateDto(
    string StudentId,
    decimal TotalAmount,
    string Currency = "EUR",
    DateTime? DueDate = null,
    string? AcademicYear = null);

public sealed record PaymentDto(
    string InvoiceId,
    decimal Amount,
    PaymentMethod Method,
    DateTime ReceivedAt,
    string? ReferenceCode,
    [property: JsonIgnore] string? Id = null);

public sealed record PaymentCreateDto(
    decimal Amount,
    PaymentMethod Method = PaymentMethod.Cash,
    string? ReferenceCode = null);

public sealed record BalanceDto(
    string StudentId,
    decimal Invoiced,
    decimal Paid,
    decimal Balance,
    string Currency);

public sealed record PayrollPeriodDto(
    int Year,
    int Month,
    PayrollStatus Status,
    [property: JsonIgnore] string? Id = null);

public sealed record PayrollPeriodCreateDto(int Year, int Month);

public sealed record PayrollRunDto(
    string PeriodId,
    DateTime RunAt,
    string? RunBy,
    decimal TotalAmount,
    [property: JsonIgnore] string? Id = null);

public sealed record PayrollRunCreateDto(string PeriodId);
