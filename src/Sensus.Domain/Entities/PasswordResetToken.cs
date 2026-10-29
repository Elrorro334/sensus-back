using System.Security.Cryptography;
using System.Text;
using Sensus.Domain.Common;

namespace Sensus.Domain.Entities;

public class PasswordResetToken : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime? UsedAtUtc { get; private set; }

    public bool IsUsed => UsedAtUtc.HasValue;
    public bool IsExpired(DateTime nowUtc) => nowUtc > ExpiresAtUtc;
    public bool IsValid(DateTime nowUtc) => !IsUsed && !IsExpired(nowUtc);

    private PasswordResetToken() : base()
    {
    }

    private PasswordResetToken(
        Guid id,
        Guid userId,
        string email,
        string tokenHash,
        DateTime createdAtUtc,
        DateTime expiresAtUtc) : base(id)
    {
        UserId = userId;
        Email = email;
        TokenHash = tokenHash;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static Result<PasswordResetToken> Create(
        Guid userId,
        string email,
        string rawToken,
        DateTime nowUtc,
        int expiryMinutes = 15)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<PasswordResetToken>(
                Error.Validation("Token.EmptyUserId", "El identificador de usuario es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<PasswordResetToken>(
                Error.Validation("Token.EmptyEmail", "El correo electrónico es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return Result.Failure<PasswordResetToken>(
                Error.Validation("Token.EmptyRawToken", "El valor del token en bruto es obligatorio."));
        }

        var tokenHash = ComputeHash(rawToken);
        var token = new PasswordResetToken(
            Guid.NewGuid(),
            userId,
            email.Trim().ToLowerInvariant(),
            tokenHash,
            nowUtc,
            nowUtc.AddMinutes(expiryMinutes));

        return Result.Success(token);
    }

    public static string ComputeHash(string rawToken)
    {
        var bytes = Encoding.UTF8.GetBytes(rawToken);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public void MarkAsUsed(DateTime nowUtc)
    {
        UsedAtUtc = nowUtc;
    }
}
