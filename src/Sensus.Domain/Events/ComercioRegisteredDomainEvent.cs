using Sensus.Domain.Common;

namespace Sensus.Domain.Events;

/// <summary>
/// Evento de dominio emitido cuando un comercio es registrado y se activa el rol comercial.
/// </summary>
public sealed record ComercioRegisteredDomainEvent(
    Guid ComercioId,
    Guid UsuarioId,
    string NombreComercial) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
