using System.Text.Json.Serialization;
using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Admin;

public sealed record UserAdminDto(
    string Id,
    string Email,
    string? FirstName,
    string? LastName,
    Role Role,
    string? TenantId,
    string Status,
    DateTime? LastLoginAt);

public sealed record UserCreateDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Role Role,
    string? TenantId = null);

public sealed record UserRoleUpdateDto(Role Role);

public sealed record TenantAdminDto(
    string Code,
    string Name,
    TenantType Type,
    TenantStatus Status,
    string Locale,
    string Timezone,
    string? AcademicYear,
    [property: JsonIgnore] string? Id = null);

public sealed record TenantCreateDto(
    string Code,
    string Name,
    TenantType Type = TenantType.School,
    string Locale = "fr-FR",
    string Timezone = "Europe/Paris",
    string? AcademicYear = null);

public sealed record TenantSettingsDto(double GradingScale, int WeekStartsOn);

public sealed record AuditLogDto(
    string Id,
    string TenantId,
    string? UserId,
    string Action,
    string ResourceType,
    string? ResourceId,
    string? IpAddress,
    DateTime OccurredAt);
