namespace Sensus.Application.Users.DTOs;

/// <summary>
/// DTO con los datos de sesión para un usuario invitado (SE-008).
/// </summary>
public sealed record GuestSessionResponseDto(
    string Token,
    string TokenType,
    DateTime ExpiresAtUtc,
    int MaxRadiusMeters,
    string Role);
