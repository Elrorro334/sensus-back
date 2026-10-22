namespace Sensus.Application.Sensors.DTOs;

/// <summary>
/// DTO inmutable para exponer información de sensores fuera del dominio.
/// </summary>
public sealed record SensorDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    string Type,
    string Status,
    decimal? LastReading,
    DateTime? LastReadingAtUtc,
    DateTime CreatedAtUtc);
