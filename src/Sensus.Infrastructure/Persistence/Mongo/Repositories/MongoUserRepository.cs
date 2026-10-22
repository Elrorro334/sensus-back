using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// Implementación del repositorio de usuarios con MongoDB Atlas y resiliencia para desarrollo local.
/// </summary>
public sealed class MongoUserRepository : IUserRepository
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoUserRepository> _logger;

    // Respaldo en memoria concurrente en caso de que el clúster Atlas aún no esté aprovisionado o sin conexión
    private static readonly ConcurrentDictionary<Guid, User> _fallbackStore = new();

    public MongoUserRepository(
        MongoDbContext context,
        ILogger<MongoUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_context.Users is not null)
        {
            try
            {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Id, id);
                var document = await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
                if (document is not null)
                {
                    return document.ToDomain();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo conectar a MongoDB Atlas. Verificando almacén local de desarrollo.");
            }
        }

        _fallbackStore.TryGetValue(id, out var fallbackUser);
        return fallbackUser;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (_context.Users is not null)
        {
            try
            {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Correo, normalizedEmail);
                var document = await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
                if (document is not null)
                {
                    return document.ToDomain();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo conectar a MongoDB Atlas. Verificando almacén local de desarrollo.");
            }
        }

        return _fallbackStore.Values.FirstOrDefault(u =>
            u.Correo.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (_context.Users is not null)
        {
            try
            {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Correo, normalizedEmail);
                var count = await _context.Users.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
                if (count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo conectar a MongoDB Atlas al verificar existencia de correo.");
            }
        }

        return _fallbackStore.Values.Any(u =>
            u.Correo.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase));
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _fallbackStore[user.Id] = user;

        if (_context.Users is not null)
        {
            try
            {
                var document = UserDocument.FromDomain(user);
                await _context.Users.InsertOneAsync(document, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo guardar en MongoDB Atlas en este momento. Se conservó en memoria.");
            }
        }
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _fallbackStore[user.Id] = user;

        if (_context.Users is not null)
        {
            try
            {
                var document = UserDocument.FromDomain(user);
                var filter = Builders<UserDocument>.Filter.Eq(u => u.Id, user.Id);
                await _context.Users.ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo actualizar en MongoDB Atlas en este momento. Se conservó en memoria.");
            }
        }
    }
}
