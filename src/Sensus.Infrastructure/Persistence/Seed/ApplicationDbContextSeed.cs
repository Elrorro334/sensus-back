using Microsoft.EntityFrameworkCore;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;

namespace Sensus.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        if (await context.Sensors.AnyAsync())
        {
            return;
        }

        var sensor1 = Sensor.Create(
            "TEMP-001",
            "Sensor de Temperatura Principal",
            "Mide la temperatura del cuarto de servidores principal.",
            SensorType.Temperature);

        var sensor2 = Sensor.Create(
            "HUM-002",
            "Sensor de Humedad Ambiente",
            "Mide la humedad relativa en el almacén de componentes.",
            SensorType.Humidity);

        var sensor3 = Sensor.Create(
            "PRES-003",
            "Sensor de Presión Hidráulica",
            "Monitorea la presión en la línea central de bombeo.",
            SensorType.Pressure);

        if (sensor1.IsSuccess && sensor2.IsSuccess && sensor3.IsSuccess)
        {
            sensor1.Value.RecordReading(23.5m, DateTime.UtcNow.AddMinutes(-10));
            sensor2.Value.RecordReading(45.2m, DateTime.UtcNow.AddMinutes(-5));
            sensor3.Value.RecordReading(101.3m, DateTime.UtcNow.AddMinutes(-2));

            await context.Sensors.AddRangeAsync([sensor1.Value, sensor2.Value, sensor3.Value]);
            await context.SaveChangesAsync();
        }
    }
}
