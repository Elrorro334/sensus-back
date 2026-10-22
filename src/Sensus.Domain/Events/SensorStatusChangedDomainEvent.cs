using Sensus.Domain.Common;
using Sensus.Domain.Enums;

namespace Sensus.Domain.Events;

public sealed record SensorStatusChangedDomainEvent(
    Guid SensorId,
    SensorStatus OldStatus,
    SensorStatus NewStatus) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
