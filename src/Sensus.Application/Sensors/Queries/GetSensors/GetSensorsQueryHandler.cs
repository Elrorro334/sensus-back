using MediatR;
using Sensus.Application.Common.Models;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Sensors.Queries.GetSensors;

public sealed class GetSensorsQueryHandler : IRequestHandler<GetSensorsQuery, Result<PagedResult<SensorDto>>>
{
    private readonly ISensorRepository _sensorRepository;

    public GetSensorsQueryHandler(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<Result<PagedResult<SensorDto>>> Handle(
        GetSensorsQuery request,
        CancellationToken cancellationToken)
    {
        var totalCount = await _sensorRepository.CountAsync(
            request.SearchTerm,
            request.Status,
            cancellationToken);

        var sensors = await _sensorRepository.GetAllAsync(
            request.SearchTerm,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var sensorDtos = sensors.Select(s => s.ToDto()).ToList();

        var pagedResult = new PagedResult<SensorDto>(
            sensorDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result.Success(pagedResult);
    }
}
