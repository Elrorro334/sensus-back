using MediatR;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Sensors.Queries.GetSensorById;

/// <summary>
/// Consulta para obtener un sensor por su identificador único.
/// </summary>
public sealed record GetSensorByIdQuery(Guid Id) : IRequest<Result<SensorDto>>;
