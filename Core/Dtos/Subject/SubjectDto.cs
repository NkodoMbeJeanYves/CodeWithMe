using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Subject;

public record SubjectDto(
    string SubjectName,
    string? Description = null,
    [property: JsonIgnore] string? SubjectId = null
);
