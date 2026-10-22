using MediatR;
using Sensus.Domain.Common;

namespace Sensus.Application.Sensors.Commands.RecordReading;

/// <summary>
/// Comando para registrar una lectura de medición en un sensor.
/// </summary>
public sealed record RecordReadingCommand(
    Guid SensorId,
    decimal Reading,
    DateTime? TimestampUtc = null) : IRequest<Result>;
