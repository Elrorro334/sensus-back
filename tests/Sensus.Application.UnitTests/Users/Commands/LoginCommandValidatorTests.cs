using FluentAssertions;
using Sensus.Application.Users.Commands.Login;

namespace Sensus.Application.UnitTests.Users.Commands;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    [InlineData(null)]
    public void Validate_Should_Fail_WhenEmailIsInvalid(string? invalidEmail)
    {
        // Arrange
        var command = new LoginCommand(invalidEmail!, "ValidPass123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Correo));
    }

    [Fact]
    public void Validate_Should_Fail_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new LoginCommand("valid@email.com", "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(LoginCommand.Password));
    }

    [Fact]
    public void Validate_Should_Succeed_WhenCommandIsValid()
    {
        // Arrange
        var command = new LoginCommand("valid@email.com", "Secret123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
