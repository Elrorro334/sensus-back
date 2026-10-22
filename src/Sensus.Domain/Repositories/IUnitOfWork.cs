namespace Sensus.Domain.Repositories;

/// <summary>
/// Contrato de Unidad de Trabajo para coordinar la persistencia atómica de cambios.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
