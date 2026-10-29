using FluentAssertions;
using Sensus.Domain.Entities;

namespace Sensus.Application.UnitTests.Users.Entities;

public class PasswordResetTokenTests
{
    [Fact]
    public void Create_ShouldGenerateValidToken_With15MinutesExpiry()
    {
        var now = new DateTime(2026, 10, 29, 12, 0, 0, DateTimeKind.Utc);
        var userId = Guid.NewGuid();
        var email = "user@sensus.com";
        var rawToken = "sample-32-byte-token-string-here";

        var token = PasswordResetToken.Create(userId, email, rawToken, now, expiryMinutes: 15).Value;

        token.UserId.Should().Be(userId);
        token.Email.Should().Be(email);
        token.TokenHash.Should().NotBeNullOrWhiteSpace();
        token.TokenHash.Should().NotBe(rawToken); // SHA-256 hasheado
        token.ExpiresAtUtc.Should().Be(now.AddMinutes(15));
        token.IsUsed.Should().BeFalse();
        token.IsValid(now).Should().BeTrue();
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenExpired()
    {
        var now = new DateTime(2026, 10, 29, 12, 0, 0, DateTimeKind.Utc);
        var token = PasswordResetToken.Create(Guid.NewGuid(), "u@s.com", "raw-token", now, 15).Value;

        token.IsValid(now.AddMinutes(14)).Should().BeTrue();
        token.IsValid(now.AddMinutes(15).AddSeconds(1)).Should().BeFalse();
    }

    [Fact]
    public void MarkAsUsed_ShouldInvalidateToken()
    {
        var now = new DateTime(2026, 10, 29, 12, 0, 0, DateTimeKind.Utc);
        var token = PasswordResetToken.Create(Guid.NewGuid(), "u@s.com", "raw-token", now, 15).Value;

        token.MarkAsUsed(now.AddMinutes(5));

        token.IsUsed.Should().BeTrue();
        token.IsValid(now.AddMinutes(6)).Should().BeFalse();
    }
}
