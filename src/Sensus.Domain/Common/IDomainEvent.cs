namespace Sensus.Domain.Common;

/// <summary>
/// Interfaz marcadora para eventos de dominio.
/// Los eventos representan hechos pasados inmutables que ocurrieron en el modelo de dominio.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}
