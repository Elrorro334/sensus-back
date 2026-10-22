using MediatR;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Sensors.Queries.GetSensorById;

public sealed class GetSensorByIdQueryHandler : IRequestHandler<GetSensorByIdQuery, Result<SensorDto>>
{
    private readonly ISensorRepository _sensorRepository;

    public GetSensorByIdQueryHandler(ISensorRepository sensorRepository)
    {
        _sensorRepository = sensorRepository;
    }

    public async Task<Result<SensorDto>> Handle(
        GetSensorByIdQuery request,
        CancellationToken cancellationToken)
    {
        var sensor = await _sensorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sensor is null)
        {
            return Result.Failure<SensorDto>(
                Error.NotFound("Sensor.NotFound", $"No se encontró el sensor con ID '{request.Id}'."));
        }

        return Result.Success(sensor.ToDto());
    }
}
