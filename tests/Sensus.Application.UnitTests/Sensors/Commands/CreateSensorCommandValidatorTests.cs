using FluentAssertions;
using Sensus.Application.Sensors.Commands.CreateSensor;
using Sensus.Domain.Enums;

namespace Sensus.Application.UnitTests.Sensors.Commands;

public class CreateSensorCommandValidatorTests
{
    private readonly CreateSensorCommandValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_Should_HaveError_WhenCodeIsInvalid(string? code)
    {
        // Arrange
        var command = new CreateSensorCommand(code!, "Sensor Valido", "Desc", SensorType.Temperature);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateSensorCommand.Code));
    }

    [Fact]
    public void Validate_Should_HaveError_WhenCodeContainsSpecialCharacters()
    {
        // Arrange
        var command = new CreateSensorCommand("TEMP@#$!", "Sensor Valido", "Desc", SensorType.Temperature);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateSensorCommand.Code));
    }

    [Fact]
    public void Validate_Should_BeValid_WhenCommandIsCorrect()
    {
        // Arrange
        var command = new CreateSensorCommand("TEMP-VALID-01", "Sensor Valido", "Desc", SensorType.Temperature);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
