using FluentAssertions;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.Logout;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class LogoutCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly LogoutCommandHandler _handler;

    public LogoutCommandHandlerTests()
    {
        _handler = new LogoutCommandHandler(_userRepository, _currentUserService);
    }

    [Fact]
    public async Task Handle_ShouldRotateSecurityStampAndUpdateUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var user = User.Create(
            "Test Logout User",
            "logout@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        var originalStamp = user.SecurityStamp;

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _handler.Handle(new LogoutCommand(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.SecurityStamp.Should().NotBe(originalStamp);
        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCallerIsNotAuthorized()
    {
        // Arrange
        _currentUserService.UserId.Returns((string?)null);

        // Act
        var result = await _handler.Handle(new LogoutCommand(), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.Unauthorized");
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserNotFound_ForIdempotency()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());
        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _handler.Handle(new LogoutCommand(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}
