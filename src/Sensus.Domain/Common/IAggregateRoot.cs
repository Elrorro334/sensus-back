namespace Sensus.Domain.Common;

/// <summary>
/// Interfaz marcadora para indicar que una entidad es la raíz de un agregado (Aggregate Root) en DDD.
/// Solo los Aggregate Roots deben tener repositorios directos.
/// </summary>
public interface IAggregateRoot
{
}
