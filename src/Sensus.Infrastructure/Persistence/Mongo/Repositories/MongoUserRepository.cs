using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// Implementación del repositorio de usuarios con MongoDB Atlas.
/// </summary>
public sealed class MongoUserRepository : IUserRepository
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoUserRepository> _logger;

    public MongoUserRepository(
        MongoDbContext context,
        ILogger<MongoUserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserDocument>.Filter.Eq(u => u.Id, id);
        var document = await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document?.ToDomain();
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var filter = Builders<UserDocument>.Filter.Eq(u => u.Correo, normalizedEmail);
        var document = await _context.Users.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document?.ToDomain();
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var filter = Builders<UserDocument>.Filter.Eq(u => u.Correo, normalizedEmail);
        var count = await _context.Users.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        return count > 0;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var document = UserDocument.FromDomain(user);
        await _context.Users.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var document = UserDocument.FromDomain(user);
        var filter = Builders<UserDocument>.Filter.Eq(u => u.Id, user.Id);
        await _context.Users.ReplaceOneAsync(filter, document, cancellationToken: cancellationToken);
    }
}
