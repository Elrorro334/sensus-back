using Sensus.Domain.Common;

namespace Sensus.Domain.Events;

public sealed record UserRegisteredDomainEvent(Guid UserId, string Correo, string NombreCompleto) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
