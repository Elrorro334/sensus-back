using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sensus.Domain.Entities;

namespace Sensus.Infrastructure.Persistence.Mongo.Documents;

public sealed class PasswordResetTokenDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    [BsonElement("userId")]
    public Guid UserId { get; set; }

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("tokenHash")]
    public string TokenHash { get; set; } = string.Empty;

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }

    [BsonElement("expiresAtUtc")]
    public DateTime ExpiresAtUtc { get; set; }

    [BsonElement("usedAtUtc")]
    public DateTime? UsedAtUtc { get; set; }

    public static PasswordResetTokenDocument FromDomain(PasswordResetToken token)
    {
        return new PasswordResetTokenDocument
        {
            Id = token.Id,
            UserId = token.UserId,
            Email = token.Email,
            TokenHash = token.TokenHash,
            CreatedAtUtc = token.CreatedAtUtc,
            ExpiresAtUtc = token.ExpiresAtUtc,
            UsedAtUtc = token.UsedAtUtc
        };
    }

    public PasswordResetToken ToDomain()
    {
        var token = PasswordResetToken.Create(
            UserId,
            Email,
            "placeholder-raw",
            CreatedAtUtc,
            15).Value;

        typeof(PasswordResetToken).GetProperty("Id")?.SetValue(token, Id);
        typeof(PasswordResetToken).GetProperty("TokenHash")?.SetValue(token, TokenHash);
        typeof(PasswordResetToken).GetProperty("ExpiresAtUtc")?.SetValue(token, ExpiresAtUtc);
        typeof(PasswordResetToken).GetProperty("UsedAtUtc")?.SetValue(token, UsedAtUtc);

        return token;
    }
}
