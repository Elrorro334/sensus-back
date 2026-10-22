using Sensus.Domain.Common;

namespace Sensus.Domain.Events;

public sealed record SensorReadingRecordedDomainEvent(
    Guid SensorId,
    decimal Reading,
    DateTime RecordedAtUtc) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
