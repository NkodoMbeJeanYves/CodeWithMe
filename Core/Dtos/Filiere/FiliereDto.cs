using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Filiere;

public sealed record FiliereDto(
    string Code,
    string Name,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record FiliereCreateDto(string Code, string Name);
