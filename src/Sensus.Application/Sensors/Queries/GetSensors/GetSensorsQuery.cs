using MediatR;
using Sensus.Application.Common.Models;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Enums;

namespace Sensus.Application.Sensors.Queries.GetSensors;

/// <summary>
/// Consulta para obtener sensores con filtros de búsqueda y paginación.
/// </summary>
public sealed record GetSensorsQuery(
    string? SearchTerm = null,
    SensorStatus? Status = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedResult<SensorDto>>>;
