using CodeWithMe.Core.Models.Enums;

namespace CodeWithMe.Core.Dtos.Auth;

/// <summary>
/// Payload du POST /v1/auth/login (contrat section 7.1 et exemple section 12).
/// </summary>
public sealed record LoginRequestV1Dto(string Email, string Password);

public sealed record AuthenticatedUserDto(
    string Id,
    string? TenantId,
    string Email,
    string? FirstName,
    string? LastName,
    Role Role,
    AuthorityLevel AuthorityLevel,
    string Status);

public sealed record LoginResponseV1Dto(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    AuthenticatedUserDto User);

public sealed record RefreshRequestV1Dto(string AccessToken, string RefreshToken);

public sealed record SwitchTenantRequestDto(string TenantId);

public sealed record MeResponseV1Dto(
    AuthenticatedUserDto User,
    IReadOnlyList<string> Permissions);
