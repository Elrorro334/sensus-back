using FluentAssertions;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.ResetPassword;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class ResetPasswordCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordResetTokenRepository _tokenRepository = Substitute.For<IPasswordResetTokenRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    private readonly ResetPasswordCommandHandler _handler;
    private readonly DateTime _nowUtc = new(2026, 10, 29, 12, 0, 0, DateTimeKind.Utc);

    public ResetPasswordCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(_nowUtc);
        _handler = new ResetPasswordCommandHandler(
            _userRepository,
            _tokenRepository,
            _passwordHasher,
            _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenTokenNotFoundOrExpired()
    {
        _tokenRepository.GetByTokenHashAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((PasswordResetToken?)null);

        var command = new ResetPasswordCommand("invalid-token", "NewPassword123!");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidResetToken");
    }

    [Fact]
    public async Task Handle_ShouldUpdatePasswordAndRotateSecurityStamp_WhenTokenIsValid()
    {
        var rawToken = "valid-token-secret";
        var user = User.Create("Oscar", "oscar@sensus.com", "old-hash", true, _nowUtc).Value;
        var oldStamp = user.SecurityStamp;

        var token = PasswordResetToken.Create(user.Id, user.Correo, rawToken, _nowUtc, 15).Value;
        var tokenHash = PasswordResetToken.ComputeHash(rawToken);

        _tokenRepository.GetByTokenHashAsync(tokenHash, Arg.Any<CancellationToken>())
            .Returns(token);
        _tokenRepository.TryConsumeAsync(tokenHash, _nowUtc, Arg.Any<CancellationToken>())
            .Returns(true);
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword("NewPassword123!")
            .Returns("new-hashed-password");

        var command = new ResetPasswordCommand(rawToken, "NewPassword123!");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        user.PasswordHash.Should().Be("new-hashed-password");
        user.SecurityStamp.Should().NotBe(oldStamp); // SecurityStamp rotado para invalidar sesiones viejas
        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenTokenIsExpiredByTime()
    {
        var rawToken = "expired-token-secret";
        var userId = Guid.NewGuid();
        // Creado hace 20 minutos
        var token = PasswordResetToken.Create(userId, "u@s.com", rawToken, _nowUtc.AddMinutes(-20), 15).Value;
        var tokenHash = PasswordResetToken.ComputeHash(rawToken);

        _tokenRepository.GetByTokenHashAsync(tokenHash, Arg.Any<CancellationToken>())
            .Returns(token);

        var command = new ResetPasswordCommand(rawToken, "NewPassword123!");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidResetToken");
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}
