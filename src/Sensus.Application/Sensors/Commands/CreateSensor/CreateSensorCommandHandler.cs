using MediatR;
using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Sensors.Commands.CreateSensor;

public sealed class CreateSensorCommandHandler : IRequestHandler<CreateSensorCommand, Result<SensorDto>>
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSensorCommandHandler(
        ISensorRepository sensorRepository,
        IUnitOfWork unitOfWork)
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SensorDto>> Handle(
        CreateSensorCommand request,
        CancellationToken cancellationToken)
    {
        var codeExists = await _sensorRepository.ExistsByCodeAsync(request.Code, cancellationToken);
        if (codeExists)
        {
            return Result.Failure<SensorDto>(
                Error.Conflict("Sensor.CodeExists", $"Ya existe un sensor registrado con el código '{request.Code}'."));
        }

        var createResult = Sensor.Create(
            request.Code,
            request.Name,
            request.Description,
            request.Type);

        if (createResult.IsFailure)
        {
            return Result.Failure<SensorDto>(createResult.Error);
        }

        var sensor = createResult.Value;

        await _sensorRepository.AddAsync(sensor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(sensor.ToDto());
    }
}
