using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Finance;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly ApiContext _ctx;
    public InvoicesController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Comptable, Role.ResponsableAdministratif, Role.Directeur)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Set<Invoice>().AsNoTracking()
            .Select(i => new InvoiceDto(i.StudentId, i.AcademicYear, i.TotalAmount, i.Currency,
                                        i.Status, i.DueDate, i.Id, i.TenantId))
            .ToContractPageAsync(opts, ct: ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var inv = await _ctx.Set<Invoice>().FindAsync(new object[] { id }, ct)
                  ?? throw NotFoundException.For("Invoice", id);
        return ApiResults.Ok(new InvoiceDto(inv.StudentId, inv.AcademicYear, inv.TotalAmount, inv.Currency,
                                            inv.Status, inv.DueDate, inv.Id, inv.TenantId));
    }

    [HttpPost]
    [AllowRoles(Role.Comptable)]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateDto dto, CancellationToken ct)
    {
        var inv = new Invoice
        {
            Id = Guid.NewGuid().ToString(),
            StudentId = dto.StudentId,
            TotalAmount = dto.TotalAmount,
            Currency = dto.Currency,
            DueDate = dto.DueDate,
            AcademicYear = dto.AcademicYear,
            Status = InvoiceStatus.Issued
        };
        _ctx.Set<Invoice>().Add(inv);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/invoices/{inv.Id}",
            new InvoiceDto(inv.StudentId, inv.AcademicYear, inv.TotalAmount, inv.Currency,
                           inv.Status, inv.DueDate, inv.Id, inv.TenantId));
    }

    [HttpPost("{id}/payments")]
    [AllowRoles(Role.Comptable)]
    public async Task<IActionResult> AddPayment(string id, [FromBody] PaymentCreateDto dto, CancellationToken ct)
    {
        var inv = await _ctx.Set<Invoice>().FindAsync(new object[] { id }, ct)
                  ?? throw NotFoundException.For("Invoice", id);
        if (dto.Amount <= 0)
            throw new BusinessRuleException("Payment amount must be positive", field: "amount");

        var payment = new Payment
        {
            Id = Guid.NewGuid().ToString(),
            InvoiceId = id,
            Amount = dto.Amount,
            Method = dto.Method,
            ReferenceCode = dto.ReferenceCode,
            ReceivedAt = DateTime.UtcNow
        };
        _ctx.Set<Payment>().Add(payment);

        var paidSoFar = await _ctx.Set<Payment>().Where(p => p.InvoiceId == id).SumAsync(p => p.Amount, ct);
        paidSoFar += dto.Amount;
        inv.Status = paidSoFar >= inv.TotalAmount
            ? InvoiceStatus.Paid
            : paidSoFar > 0 ? InvoiceStatus.PartiallyPaid : InvoiceStatus.Issued;

        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/invoices/{id}/payments/{payment.Id}",
            new PaymentDto(payment.InvoiceId, payment.Amount, payment.Method, payment.ReceivedAt,
                           payment.ReferenceCode, payment.Id));
    }
}
