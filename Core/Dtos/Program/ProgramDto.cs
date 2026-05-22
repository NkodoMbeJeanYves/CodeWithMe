using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Program;

public record class ProgramDto
{
    /// <summary>
    /// Identifiant généré automatiquement par la base.
    /// ⚠️ Ne pas renseigner lors de la création.
    /// </summary>
    [JsonIgnore]
    public string? ProgramId { get; init; }

    public required string Name { get; set; }
    public required string SchoolId { get; set; }
};
