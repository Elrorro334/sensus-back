using Sensus.Domain.Entities;

namespace Sensus.Domain.Repositories;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
    Task InvalidateExistingTokensForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PasswordResetToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<bool> TryConsumeAsync(string tokenHash, DateTime nowUtc, CancellationToken cancellationToken = default);
}
