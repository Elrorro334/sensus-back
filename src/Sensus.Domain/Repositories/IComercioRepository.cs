using Sensus.Domain.Entities;

namespace Sensus.Domain.Repositories;

/// <summary>
/// Contrato de repositorio para la entidad de dominio Comercio.
/// </summary>
public interface IComercioRepository
{
    Task<Comercio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Comercio?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comercio>> GetNearbyAsync(double latitud, double longitud, int radioMetros, CancellationToken cancellationToken = default);
    Task AddAsync(Comercio comercio, CancellationToken cancellationToken = default);
    Task UpdateAsync(Comercio comercio, CancellationToken cancellationToken = default);
    Task DeleteByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);
}
