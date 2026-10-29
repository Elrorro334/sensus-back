using FluentAssertions;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.UpdateProfile;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class UpdateProfileCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly UpdateProfileCommandHandler _handler;

    public UpdateProfileCommandHandlerTests()
    {
        _handler = new UpdateProfileCommandHandler(_userRepository, _currentUserService);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProfileSuccessfully_WhenCallerIsAuthorized()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var existingUser = User.Create(
            "Old Name",
            "user@sensus.com",
            "hashed-pw",
            true,
            DateTime.UtcNow).Value;

        typeof(User).GetProperty("Id")?.SetValue(existingUser, userId);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(existingUser);

        var command = new UpdateProfileCommand("New Name", "https://res.cloudinary.com/sensus/image/upload/avatar.jpg");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.NombreCompleto.Should().Be("New Name");
        result.Value.FotoPerfilUrl.Should().Be("https://res.cloudinary.com/sensus/image/upload/avatar.jpg");
        await _userRepository.Received(1).UpdateAsync(existingUser, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenCallerHasNoUserId()
    {
        _currentUserService.UserId.Returns((string?)null);

        var command = new UpdateProfileCommand("New Name", null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.Unauthorized");
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());
        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var command = new UpdateProfileCommand("New Name", null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.NotFound");
    }
}
