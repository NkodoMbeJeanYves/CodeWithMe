using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Classe;

public sealed record ClasseDto(
    string Code,
    string Name,
    string? Level = null,
    string? FiliereId = null,
    string? AcademicYear = null,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record ClasseCreateDto(
    string Code,
    string Name,
    string? Level = null,
    string? FiliereId = null,
    string? AcademicYear = null);
