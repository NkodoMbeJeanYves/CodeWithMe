using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Room;

public sealed record RoomDto(
    string Code,
    string Name,
    int? Capacity = null,
    [property: JsonIgnore] string? Id = null,
    string? TenantId = null);

public sealed record RoomCreateDto(string Code, string Name, int? Capacity = null);
