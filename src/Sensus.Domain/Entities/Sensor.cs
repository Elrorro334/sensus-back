using Sensus.Domain.Common;
using Sensus.Domain.Enums;
using Sensus.Domain.Events;

namespace Sensus.Domain.Entities;

/// <summary>
/// Entidad Sensor (Aggregate Root) con lógica de negocio rica e invariantes protegidas.
/// </summary>
public class Sensor : BaseAuditableEntity, IAggregateRoot
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public SensorType Type { get; private set; }
    public SensorStatus Status { get; private set; }
    public decimal? LastReading { get; private set; }
    public DateTime? LastReadingAtUtc { get; private set; }
    public bool IsActive => Status == SensorStatus.Active;

    // Constructor privado para EF Core / ORM
    private Sensor() : base()
    {
    }

    private Sensor(
        Guid id,
        string code,
        string name,
        string description,
        SensorType type) : base(id)
    {
        Code = code;
        Name = name;
        Description = description;
        Type = type;
        Status = SensorStatus.Active;
    }

    /// <summary>
    /// Factory method para crear una nueva instancia con validación de invariantes.
    /// </summary>
    public static Result<Sensor> Create(
        string code,
        string name,
        string description,
        SensorType type)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<Sensor>(
                Error.Validation("Sensor.CodeEmpty", "El código del sensor no puede estar vacío."));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Sensor>(
                Error.Validation("Sensor.NameEmpty", "El nombre del sensor no puede estar vacío."));
        }

        var sensor = new Sensor(
            Guid.NewGuid(),
            code.Trim().ToUpperInvariant(),
            name.Trim(),
            description?.Trim() ?? string.Empty,
            type);

        sensor.AddDomainEvent(new SensorCreatedDomainEvent(sensor.Id, sensor.Code));

        return Result.Success(sensor);
    }

    /// <summary>
    /// Actualiza los detalles del sensor respetando las reglas de negocio.
    /// </summary>
    public Result UpdateDetails(string name, string description, SensorType type)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(
                Error.Validation("Sensor.NameEmpty", "El nombre del sensor no puede estar vacío."));
        }

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Type = type;

        return Result.Success();
    }

    /// <summary>
    /// Registra una nueva lectura en el sensor.
    /// </summary>
    public Result RecordReading(decimal reading, DateTime timestampUtc)
    {
        if (Status != SensorStatus.Active)
        {
            return Result.Failure(
                Error.Conflict("Sensor.NotActive", $"No se pueden registrar lecturas en un sensor con estado '{Status}'."));
        }

        LastReading = reading;
        LastReadingAtUtc = timestampUtc;

        AddDomainEvent(new SensorReadingRecordedDomainEvent(Id, reading, timestampUtc));

        return Result.Success();
    }

    /// <summary>
    /// Activa el sensor.
    /// </summary>
    public Result Activate()
    {
        if (Status == SensorStatus.Active)
        {
            return Result.Success();
        }

        var oldStatus = Status;
        Status = SensorStatus.Active;

        AddDomainEvent(new SensorStatusChangedDomainEvent(Id, oldStatus, Status));
        return Result.Success();
    }

    /// <summary>
    /// Desactiva el sensor.
    /// </summary>
    public Result Deactivate()
    {
        if (Status == SensorStatus.Inactive)
        {
            return Result.Success();
        }

        var oldStatus = Status;
        Status = SensorStatus.Inactive;

        AddDomainEvent(new SensorStatusChangedDomainEvent(Id, oldStatus, Status));
        return Result.Success();
    }

    /// <summary>
    /// Coloca el sensor en mantenimiento.
    /// </summary>
    public Result SetMaintenance()
    {
        var oldStatus = Status;
        Status = SensorStatus.Maintenance;

        AddDomainEvent(new SensorStatusChangedDomainEvent(Id, oldStatus, Status));
        return Result.Success();
    }
}
