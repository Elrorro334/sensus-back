using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using NSubstitute;
using Sensus.Application.Users.Commands.RequestPasswordReset;
using Sensus.Application.Users.Commands.ResetPassword;
using Sensus.Domain.Entities;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Authentication;

public class PasswordResetEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public PasswordResetEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Recover_ShouldReturnAcceptedAndSendEmail_WhenUserExists()
    {
        // Arrange
        var user = User.Create(
            "User Recover Test",
            "recover.test@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByEmailAsync("recover.test@sensus.com", Arg.Any<CancellationToken>())
            .Returns(user);

        var command = new RequestPasswordResetCommand("recover.test@sensus.com");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/recover", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        await _factory.PasswordResetTokenRepository.Received(1)
            .AddAsync(Arg.Any<PasswordResetToken>(), Arg.Any<CancellationToken>());
        _factory.FakeEmailSender.SentEmails.Should().Contain(e => e.To == "recover.test@sensus.com");
    }

    [Fact]
    public async Task Recover_ShouldReturnAcceptedAndNotSendEmail_WhenUserDoesNotExist()
    {
        // Anti-enumeration test: debe retornar 202 Accepted pero no enviar correo
        _factory.UserRepository.GetByEmailAsync("nonexistent@sensus.com", Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var initialCount = _factory.FakeEmailSender.SentEmails.Count;
        var command = new RequestPasswordResetCommand("nonexistent@sensus.com");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/recover", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        _factory.FakeEmailSender.SentEmails.Count.Should().Be(initialCount);
    }

    [Fact]
    public async Task Recover_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        var command = new RequestPasswordResetCommand("invalid-email-format");

        var response = await _client.PostAsJsonAsync("/api/v1/auth/recover", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnNoContent_WhenTokenIsValid()
    {
        // Arrange
        var user = User.Create(
            "Reset Target User",
            "target.reset@sensus.com",
            "old-hashed-password",
            true,
            DateTime.UtcNow).Value;

        var rawToken = "sample-secret-token-1234567890123456";
        var tokenHash = PasswordResetToken.ComputeHash(rawToken);

        var resetToken = PasswordResetToken.Create(
            user.Id,
            user.Correo,
            rawToken,
            _factory.FakeClock.UtcNow,
            15).Value;

        _factory.PasswordResetTokenRepository.TryConsumeAsync(tokenHash, Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(true);
        _factory.PasswordResetTokenRepository.GetByTokenHashAsync(tokenHash, Arg.Any<CancellationToken>())
            .Returns(resetToken);
        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var command = new ResetPasswordCommand(rawToken, "NuevaPass1234#");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/reset-password", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await _factory.UserRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ResetPassword_ShouldReturnBadRequest_WhenTokenIsExpiredOrAlreadyUsed()
    {
        // Arrange
        var rawToken = "already-used-or-expired-token";
        var tokenHash = PasswordResetToken.ComputeHash(rawToken);

        _factory.PasswordResetTokenRepository.TryConsumeAsync(tokenHash, Arg.Any<DateTime>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var command = new ResetPasswordCommand(rawToken, "NuevaPass1234#");

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/auth/reset-password", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
