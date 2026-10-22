using Sensus.Domain.Common;

namespace Sensus.Domain.Events;

public sealed record SensorCreatedDomainEvent(Guid SensorId, string Code) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
