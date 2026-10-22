using FluentAssertions;
using Sensus.Application.Users.Commands.RegisterUser;

namespace Sensus.Application.UnitTests.Users.Commands;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@domain")]
    [InlineData("@nodomain.com")]
    [InlineData("")]
    public void Validate_Should_Fail_WhenEmailIsInvalid(string invalidEmail)
    {
        // Arrange
        var command = new RegisterUserCommand("Juan Perez", invalidEmail, "SecurePass123!", true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserCommand.Correo));
    }

    [Theory]
    [InlineData("short1A")]             // Menos de 8 caracteres
    [InlineData("alllowercase123!")]     // Sin mayúsculas
    [InlineData("ALLUPPERCASE123!")]     // Sin minúsculas
    [InlineData("NoNumbersHere!")]       // Sin números
    public void Validate_Should_Fail_WhenPasswordDoesNotMeetComplexity(string weakPassword)
    {
        // Arrange
        var command = new RegisterUserCommand("Juan Perez", "juan@test.com", weakPassword, true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserCommand.Password));
    }

    [Fact]
    public void Validate_Should_Fail_WhenTermsAreNotAccepted()
    {
        // Arrange
        var command = new RegisterUserCommand("Juan Perez", "juan@test.com", "SecurePass123!", false);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserCommand.AceptoTerminos));
    }

    [Fact]
    public void Validate_Should_Succeed_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterUserCommand("Juan Perez", "juan@test.com", "SecurePass123!", true);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
