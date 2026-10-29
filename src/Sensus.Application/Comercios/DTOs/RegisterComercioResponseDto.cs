namespace Sensus.Application.Comercios.DTOs;

/// <summary>
/// DTO de respuesta para el registro y activación de rol comercial (SE-003 & SE-004).
/// </summary>
public sealed record RegisterComercioResponseDto(
    string Mensaje,
    Guid ComercioId,
    bool EsComerciante,
    IReadOnlyList<string> Roles,
    ComercioDto Comercio,
    string Token,
    string TokenType,
    DateTime ExpiresAtUtc);
