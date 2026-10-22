namespace Sensus.Application.Users.DTOs;

public sealed record UserDto(
    Guid Id,
    string NombreCompleto,
    string Correo,
    IReadOnlyList<string> Roles,
    bool Estatus,
    DateTime FechaRegistro,
    DateTime? UltimoAcceso);
