using Sensus.Domain.Common;

namespace Sensus.Domain.Events;

public sealed record UserLoggedInDomainEvent(Guid UserId, DateTime LoginTimeUtc) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
