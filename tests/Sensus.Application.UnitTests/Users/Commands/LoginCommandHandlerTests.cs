using FluentAssertions;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.Login;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class LoginCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        _dateTimeProvider.UtcNow.Returns(new DateTime(2026, 10, 22, 12, 0, 0, DateTimeKind.Utc));

        _handler = new LoginCommandHandler(
            _userRepository,
            _passwordHasher,
            _jwtTokenGenerator,
            _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var user = User.Create(
            "Carlos Diaz",
            "carlos@sensus.com",
            "$2a$11$mockedBCryptHash",
            true,
            DateTime.UtcNow).Value;

        var command = new LoginCommand("carlos@sensus.com", "CorrectPassword123!");

        _userRepository.GetByEmailAsync(command.Correo, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(command.Password, user.PasswordHash)
            .Returns(true);

        _jwtTokenGenerator.GenerateToken(user)
            .Returns(("valid.jwt.token", DateTime.UtcNow.AddHours(24)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Token.Should().Be("valid.jwt.token");
        result.Value.User.Correo.Should().Be("carlos@sensus.com");

        await _userRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var command = new LoginCommand("noexiste@sensus.com", "Password123!");

        _userRepository.GetByEmailAsync(command.Correo, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidCredentials");
        result.Error.Message.Should().Be("Correo o contraseña incorrectos.");
    }

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = User.Create(
            "Carlos Diaz",
            "carlos@sensus.com",
            "$2a$11$mockedBCryptHash",
            true,
            DateTime.UtcNow).Value;

        var command = new LoginCommand("carlos@sensus.com", "WrongPassword123!");

        _userRepository.GetByEmailAsync(command.Correo, Arg.Any<CancellationToken>())
            .Returns(user);

        _passwordHasher.VerifyPassword(command.Password, user.PasswordHash)
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidCredentials");
        result.Error.Message.Should().Be("Correo o contraseña incorrectos.");
    }

    [Fact]
    public async Task Handle_Should_ReturnForbidden_WhenUserIsDeactivated()
    {
        // Arrange
        var user = User.Create(
            "Inactivo User",
            "inactivo@sensus.com",
            "$2a$11$mockedBCryptHash",
            true,
            DateTime.UtcNow).Value;
        user.Deactivate();

        var command = new LoginCommand("inactivo@sensus.com", "Password123!");

        _userRepository.GetByEmailAsync(command.Correo, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.UserDeactivated");
    }
}
