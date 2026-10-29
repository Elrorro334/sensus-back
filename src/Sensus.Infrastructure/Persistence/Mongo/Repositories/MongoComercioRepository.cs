using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// Implementación de repositorio para Comercio con persistencia directa en MongoDB Atlas.
/// Soporta consultas geoespaciales 2dsphere para búsqueda de proximidad (SE-008).
/// </summary>
public sealed class MongoComercioRepository : IComercioRepository
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoComercioRepository> _logger;

    public MongoComercioRepository(
        MongoDbContext context,
        ILogger<MongoComercioRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Comercio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ComercioDocument>.Filter.Eq(c => c.Id, id);
        var document = await _context.Comercios.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document?.ToDomain();
    }

    public async Task<Comercio?> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ComercioDocument>.Filter.Eq(c => c.UsuarioId, usuarioId);
        var document = await _context.Comercios.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document?.ToDomain();
    }

    public async Task<bool> ExistsByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ComercioDocument>.Filter.Eq(c => c.UsuarioId, usuarioId);
        var count = await _context.Comercios.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task<IReadOnlyList<Comercio>> GetNearbyAsync(
        double latitud,
        double longitud,
        int radioMetros,
        CancellationToken cancellationToken = default)
    {
        var point = GeoJson.Point(GeoJson.Geographic(longitud, latitud));
        var filter = Builders<ComercioDocument>.Filter.And(
            Builders<ComercioDocument>.Filter.Eq(c => c.Activo, true),
            Builders<ComercioDocument>.Filter.NearSphere(c => c.Ubicacion, point, maxDistance: radioMetros)
        );

        var documents = await _context.Comercios
            .Find(filter)
            .Limit(50)
            .ToListAsync(cancellationToken);

        return documents.Select(d => d.ToDomain()).ToList();
    }

    public async Task AddAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        var document = ComercioDocument.FromDomain(comercio);
        await _context.Comercios.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(Comercio comercio, CancellationToken cancellationToken = default)
    {
        var document = ComercioDocument.FromDomain(comercio);
        var filter = Builders<ComercioDocument>.Filter.Eq(c => c.Id, comercio.Id);
        await _context.Comercios.ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
    }

    public async Task DeleteByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ComercioDocument>.Filter.Eq(c => c.UsuarioId, usuarioId);
        await _context.Comercios.DeleteManyAsync(filter, cancellationToken);
    }
}
