using System.Collections.Concurrent;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.TestKit.Fakes;

public class InMemoryPasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly ConcurrentDictionary<Guid, PasswordResetToken> _tokens = new();

    public Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
    {
        _tokens[token.Id] = token;
        return Task.CompletedTask;
    }

    public Task InvalidateExistingTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // El repositorio no necesita mutar campos privados, sólo la verificación en TryConsume/IsValid
        return Task.CompletedTask;
    }

    public Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        var token = _tokens.Values.FirstOrDefault(t => t.TokenHash == tokenHash);
        return Task.FromResult(token);
    }

    public Task<bool> TryConsumeAsync(string tokenHash, DateTime nowUtc, CancellationToken cancellationToken = default)
    {
        var token = _tokens.Values.FirstOrDefault(t => t.TokenHash == tokenHash);
        if (token is null || !token.IsValid(nowUtc))
        {
            return Task.FromResult(false);
        }

        token.MarkAsUsed(nowUtc);
        return Task.FromResult(true);
    }
}
