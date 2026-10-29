using System.Collections.Concurrent;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.TestKit.Fakes;

public class InMemoryComercioRepository : IComercioRepository
{
    private readonly ConcurrentDictionary<Guid, Comercio> _comercios = new();

    public Task<Comercio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _comercios.TryGetValue(id, out var comercio);
        return Task.FromResult(comercio);
    }

    public Task<Comercio?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var comercio = _comercios.Values.FirstOrDefault(c => c.UsuarioId == usuarioId);
        return Task.FromResult(comercio);
    }

    public Task<bool> ExistsByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var exists = _comercios.Values.Any(c => c.UsuarioId == usuarioId);
        return Task.FromResult(exists);
    }

    public Task<IReadOnlyList<Comercio>> GetNearbyAsync(double latitud, double longitud, int radioMetros, CancellationToken cancellationToken = default)
    {
        var list = _comercios.Values
            .Where(c => CalculateDistanceMeters(latitud, longitud, c.Latitud, c.Longitud) <= radioMetros)
            .OrderBy(c => CalculateDistanceMeters(latitud, longitud, c.Latitud, c.Longitud))
            .ToList();

        return Task.FromResult<IReadOnlyList<Comercio>>(list);
    }

    public Task AddAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        _comercios[comercio.Id] = comercio;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        _comercios[comercio.Id] = comercio;
        return Task.CompletedTask;
    }

    public Task DeleteByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var toRemove = _comercios.Values.Where(c => c.UsuarioId == usuarioId).ToList();
        foreach (var c in toRemove)
        {
            _comercios.TryRemove(c.Id, out _);
        }
        return Task.CompletedTask;
    }

    private static double CalculateDistanceMeters(double lat1, double lon1, double lat2, double lon2)
    {
        const double r = 6371000.0;
        var dLat = (lat2 - lat1) * Math.PI / 180.0;
        var dLon = (lon2 - lon1) * Math.PI / 180.0;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return r * c;
    }
}
