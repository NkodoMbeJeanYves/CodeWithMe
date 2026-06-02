using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Edu;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/modeles-message")]
public class ModelesMessageController : ControllerBase
{
    private readonly ApiContext _ctx;
    public ModelesMessageController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.SuperAdmin, Role.Directeur, Role.Secretaire)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var query = _ctx.EduModelesMessage.AsNoTracking();
        var type = Request.Query["type"].FirstOrDefault();
        if (type is not null) query = query.Where(m => m.Type == type);
        if (bool.TryParse(Request.Query["actif"].FirstOrDefault(), out var actif))
            query = query.Where(m => m.Actif == actif);
        var (items, meta) = await query.ToContractPageAsync(opts, q => m => m.Nom.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var m = await _ctx.EduModelesMessage.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ModeleMessage", id.ToString());
        return ApiResults.Ok(m);
    }

    [HttpPost]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Create([FromBody] ModeleMessage dto, CancellationToken ct)
    {
        dto.Id = Guid.NewGuid();
        _ctx.EduModelesMessage.Add(dto);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/modeles-message/{dto.Id}", dto);
    }

    [HttpPatch("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Update(Guid id, [FromBody] ModeleMessage dto, CancellationToken ct)
    {
        var m = await _ctx.EduModelesMessage.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ModeleMessage", id.ToString());
        if (dto.Nom is not null) m.Nom = dto.Nom;
        if (dto.Corps is not null) m.Corps = dto.Corps;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Ok(m);
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.SuperAdmin, Role.Directeur)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var m = await _ctx.EduModelesMessage.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("ModeleMessage", id.ToString());
        m.Actif = false;
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
