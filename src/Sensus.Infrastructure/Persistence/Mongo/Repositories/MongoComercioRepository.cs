using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// Implementación de repositorio para Comercio con persistencia en MongoDB Atlas y fallback local.
/// </summary>
public sealed class MongoComercioRepository : IComercioRepository
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoComercioRepository> _logger;

    // Respaldo en memoria concurrente para desarrollo local / pruebas offline
    private static readonly ConcurrentDictionary<Guid, Comercio> _fallbackStore = new();

    public MongoComercioRepository(
        MongoDbContext context,
        ILogger<MongoComercioRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Comercio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ComercioDocument>.Filter.Eq(c => c.Id, id);
            var document = await _context.Comercios.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (document is not null)
            {
                return document.ToDomain();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo consultar comercio en MongoDB Atlas. Buscando en respaldo local.");
        }

        _fallbackStore.TryGetValue(id, out var fallbackComercio);
        return fallbackComercio;
    }

    public async Task<Comercio?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ComercioDocument>.Filter.Eq(c => c.UsuarioId, usuarioId);
            var document = await _context.Comercios.Find(filter).FirstOrDefaultAsync(cancellationToken);
            if (document is not null)
            {
                return document.ToDomain();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo consultar comercio por UsuarioId en MongoDB Atlas. Buscando en respaldo local.");
        }

        return _fallbackStore.Values.FirstOrDefault(c => c.UsuarioId == usuarioId);
    }

    public async Task<bool> ExistsByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<ComercioDocument>.Filter.Eq(c => c.UsuarioId, usuarioId);
            var count = await _context.Comercios.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            if (count > 0)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo verificar existencia de comercio en MongoDB Atlas. Verificando en respaldo local.");
        }

        return _fallbackStore.Values.Any(c => c.UsuarioId == usuarioId);
    }

    public async Task AddAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        _fallbackStore[comercio.Id] = comercio;

        try
        {
            var document = ComercioDocument.FromDomain(comercio);
            await _context.Comercios.InsertOneAsync(document, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo guardar comercio en MongoDB Atlas. Conservado en memoria local.");
        }
    }

    public async Task UpdateAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        _fallbackStore[comercio.Id] = comercio;

        try
        {
            var document = ComercioDocument.FromDomain(comercio);
            var filter = Builders<ComercioDocument>.Filter.Eq(c => c.Id, comercio.Id);
            await _context.Comercios.ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo actualizar comercio en MongoDB Atlas. Conservado en memoria local.");
        }
    }
}
