using Sensus.Application.Sensors.DTOs;
using Sensus.Domain.Entities;

namespace Sensus.Application.Sensors;

public static class SensorMappingExtensions
{
    public static SensorDto ToDto(this Sensor sensor)
    {
        return new SensorDto(
            sensor.Id,
            sensor.Code,
            sensor.Name,
            sensor.Description,
            sensor.Type.ToString(),
            sensor.Status.ToString(),
            sensor.LastReading,
            sensor.LastReadingAtUtc,
            sensor.CreatedAtUtc);
    }
}
