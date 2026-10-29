using FluentAssertions;
using Sensus.Application.Comercios.Commands.RegisterComercio;

namespace Sensus.Application.UnitTests.Comercios.Commands;

public class RegisterComercioCommandValidatorTests
{
    private readonly RegisterComercioCommandValidator _validator = new();

    [Fact]
    public void Validate_Should_BeValid_WhenAllFieldsAreValid()
    {
        // Arrange
        var command = new RegisterComercioCommand(
            "Mi Tienda Don Pepe",
            "Calle Independencia #42, Col. Centro",
            "06000");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("A")]
    public void Validate_Should_Fail_WhenNombreComercialIsInvalid(string nombre)
    {
        var command = new RegisterComercioCommand(nombre, "Av. Principal 123", "06000");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterComercioCommand.NombreComercial));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1234")] // Menos de 5 caracteres
    public void Validate_Should_Fail_WhenDireccionIsTooShortOrEmpty(string direccion)
    {
        var command = new RegisterComercioCommand("Mi Tienda", direccion, "06000");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterComercioCommand.Direccion));
    }

    [Theory]
    [InlineData("")]
    [InlineData("12")] // Demasiado corto
    [InlineData("12345678901")] // Más de 10 caracteres
    public void Validate_Should_Fail_WhenCodigoPostalIsInvalid(string cp)
    {
        var command = new RegisterComercioCommand("Mi Tienda", "Av. Principal 123", cp);
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterComercioCommand.CodigoPostal));
    }
}
