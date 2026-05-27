using System.Text.Json.Serialization;

namespace CodeWithMe.Core.Dtos.Communication;

public sealed record AnnouncementDto(
    string Title,
    string Body,
    DateTime? PublishedAt,
    DateTime? ExpiresAt,
    [property: JsonIgnore] string? Id = null);

public sealed record AnnouncementCreateDto(
    string Title,
    string Body,
    DateTime? ExpiresAt = null);

public sealed record MessageDto(
    string FromUserId,
    string ToUserId,
    string? Subject,
    string Body,
    DateTime? ReadAt,
    DateTime? CreatedAt,
    [property: JsonIgnore] string? Id = null);

public sealed record MessageCreateDto(
    string ToUserId,
    string Body,
    string? Subject = null);

public sealed record NotificationDto(
    string UserId,
    string Type,
    string Title,
    string? Body,
    DateTime? ReadAt,
    DateTime? CreatedAt,
    [property: JsonIgnore] string? Id = null);
