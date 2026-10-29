using FluentAssertions;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.DeleteAccount;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class DeleteAccountCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IComercioRepository _comercioRepository = Substitute.For<IComercioRepository>();
    private readonly IPasswordResetTokenRepository _tokenRepository = Substitute.For<IPasswordResetTokenRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IProfileImageStorage _imageStorage = Substitute.For<IProfileImageStorage>();
    private readonly DeleteAccountCommandHandler _handler;

    public DeleteAccountCommandHandlerTests()
    {
        _handler = new DeleteAccountCommandHandler(
            _userRepository,
            _comercioRepository,
            _tokenRepository,
            _currentUserService,
            _passwordHasher,
            _imageStorage);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUserComercioAndTokens_WhenPasswordIsCorrect()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var user = User.Create(
            "Target User",
            "target@sensus.com",
            "hashed-secret",
            true,
            DateTime.UtcNow).Value;
        typeof(User).GetProperty("Id")?.SetValue(user, userId);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword("CorrectPassword123#", "hashed-secret")
            .Returns(true);

        var command = new DeleteAccountCommand("CorrectPassword123#");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _comercioRepository.Received(1).DeleteByUsuarioIdAsync(userId, Arg.Any<CancellationToken>());
        await _tokenRepository.Received(1).InvalidateExistingTokensForUserAsync(userId, Arg.Any<CancellationToken>());
        await _userRepository.Received(1).DeleteAsync(userId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var user = User.Create(
            "Target User",
            "target@sensus.com",
            "hashed-secret",
            true,
            DateTime.UtcNow).Value;
        typeof(User).GetProperty("Id")?.SetValue(user, userId);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword("WrongPassword", "hashed-secret")
            .Returns(false);

        var command = new DeleteAccountCommand("WrongPassword");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidCredentials");
        await _userRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
    }
}
