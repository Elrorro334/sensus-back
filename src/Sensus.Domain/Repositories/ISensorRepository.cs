using Sensus.Domain.Entities;
using Sensus.Domain.Enums;

namespace Sensus.Domain.Repositories;

/// <summary>
/// Contrato del repositorio para la raíz de agregado Sensor.
/// En Clean Architecture, las interfaces de repositorio residen en el Dominio.
/// </summary>
public interface ISensorRepository
{
    Task<Sensor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Sensor?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Sensor>> GetAllAsync(
        string? searchTerm = null,
        SensorStatus? status = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(
        string? searchTerm = null,
        SensorStatus? status = null,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(Sensor sensor, CancellationToken cancellationToken = default);
    void Update(Sensor sensor);
    void Delete(Sensor sensor);
}
