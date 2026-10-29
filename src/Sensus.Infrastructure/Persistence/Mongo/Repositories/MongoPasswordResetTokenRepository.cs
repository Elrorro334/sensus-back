using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Persistence.Mongo.Documents;

namespace Sensus.Infrastructure.Persistence.Mongo.Repositories;

/// <summary>
/// Implementación con MongoDB para tokens de recuperación de contraseña (SE-005).
/// </summary>
public sealed class MongoPasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoPasswordResetTokenRepository> _logger;

    public MongoPasswordResetTokenRepository(
        MongoDbContext context,
        ILogger<MongoPasswordResetTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        var document = PasswordResetTokenDocument.FromDomain(token);
        await _context.PasswordResetTokens.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task InvalidateExistingTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PasswordResetTokenDocument>.Filter.And(
            Builders<PasswordResetTokenDocument>.Filter.Eq(t => t.UserId, userId),
            Builders<PasswordResetTokenDocument>.Filter.Eq(t => t.UsedAtUtc, null)
        );

        var update = Builders<PasswordResetTokenDocument>.Update.Set(t => t.UsedAtUtc, DateTime.UtcNow);
        await _context.PasswordResetTokens.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PasswordResetTokenDocument>.Filter.Eq(t => t.TokenHash, tokenHash);
        var document = await _context.PasswordResetTokens.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document?.ToDomain();
    }

    public async Task<bool> TryConsumeAsync(string tokenHash, DateTime nowUtc, CancellationToken cancellationToken = default)
    {
        var filter = Builders<PasswordResetTokenDocument>.Filter.And(
            Builders<PasswordResetTokenDocument>.Filter.Eq(t => t.TokenHash, tokenHash),
            Builders<PasswordResetTokenDocument>.Filter.Eq(t => t.UsedAtUtc, null),
            Builders<PasswordResetTokenDocument>.Filter.Gt(t => t.ExpiresAtUtc, nowUtc)
        );

        var update = Builders<PasswordResetTokenDocument>.Update.Set(t => t.UsedAtUtc, nowUtc);

        var options = new FindOneAndUpdateOptions<PasswordResetTokenDocument>
        {
            ReturnDocument = ReturnDocument.After
        };

        var result = await _context.PasswordResetTokens.FindOneAndUpdateAsync(
            filter,
            update,
            options,
            cancellationToken);

        return result is not null;
    }
}
