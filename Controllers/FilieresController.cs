using CodeWithMe.Core;
using CodeWithMe.Core.DataExtensions;
using CodeWithMe.Core.Dtos.Filiere;
using CodeWithMe.Core.Exceptions;
using CodeWithMe.Core.Models.Enums;
using CodeWithMe.Core.Query;
using CodeWithMe.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Controllers;

[ApiController]
[Route("v1/filieres")]
public class FilieresController : ControllerBase
{
    private readonly ApiContext _ctx;
    public FilieresController(ApiContext ctx) { _ctx = ctx; }

    [HttpGet]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique, Role.ResponsableAdministratif,
                Role.Secretaire, Role.Enseignant)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var opts = QueryOptions.FromRequest(Request);
        var (items, meta) = await _ctx.Filieres.AsNoTracking().Select(f => f.ToDto())
            .ToContractPageAsync(opts, q => f => f.Name.Contains(q) || f.Code.Contains(q), ct);
        return ApiResults.Page(items, meta, Request);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken ct)
    {
        var f = await _ctx.Filieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Filiere", id);
        return ApiResults.Ok(f.ToDto());
    }

    [HttpPost]
    [AllowRoles(Role.Directeur, Role.DirecteurPedagogique)]
    public async Task<IActionResult> Create([FromBody] FiliereCreateDto dto, CancellationToken ct)
    {
        var entity = dto.ToEntity();
        _ctx.Filieres.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.Created($"/v1/filieres/{entity.Id}", entity.ToDto());
    }

    [HttpDelete("{id}")]
    [AllowRoles(Role.Directeur)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var f = await _ctx.Filieres.FindAsync(new object[] { id }, ct)
                ?? throw NotFoundException.For("Filiere", id);
        _ctx.Filieres.Remove(f);
        await _ctx.SaveChangesAsync(ct);
        return ApiResults.NoContent();
    }
}
