using MediatR;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Enums;

namespace Sensus.Application.Sensors.Commands.CreateSensor;

/// <summary>
/// Comando para registrar un nuevo sensor.
/// </summary>
public sealed record CreateSensorCommand(
    string Code,
    string Name,
    string Description,
    SensorType Type) : IRequest<Result<SensorDto>>;
