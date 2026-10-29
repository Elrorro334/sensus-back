namespace Sensus.Application.Comercios.DTOs;

/// <summary>
/// DTO con los datos de detalle de un comercio.
/// </summary>
public sealed record ComercioDto(
    Guid Id,
    Guid UsuarioId,
    string NombreComercial,
    string Direccion,
    string CodigoPostal,
    double Latitud,
    double Longitud,
    bool Activo,
    DateTime FechaRegistro);
