using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Sensors.Commands.RecordReading;

public sealed class RecordReadingCommandHandler : IRequestHandler<RecordReadingCommand, Result>
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RecordReadingCommandHandler(
        ISensorRepository sensorRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        RecordReadingCommand request,
        CancellationToken cancellationToken)
    {
        var sensor = await _sensorRepository.GetByIdAsync(request.SensorId, cancellationToken);
        if (sensor is null)
        {
            return Result.Failure(
                Error.NotFound("Sensor.NotFound", $"No se encontró el sensor con ID '{request.SensorId}'."));
        }

        var timestamp = request.TimestampUtc ?? _dateTimeProvider.UtcNow;
        var recordResult = sensor.RecordReading(request.Reading, timestamp);

        if (recordResult.IsFailure)
        {
            return recordResult;
        }

        _sensorRepository.Update(sensor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
