namespace Sensus.Application.Comercios.DTOs;

/// <summary>
/// DTO que representa el horario de un día de la semana.
/// </summary>
public sealed record HorarioDiaDto(
    DayOfWeek Dia,
    string? HoraApertura,
    string? HoraCierre,
    bool EstaCerrado);

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
    DateTime FechaRegistro,
    IReadOnlyList<HorarioDiaDto>? Horarios = null);
