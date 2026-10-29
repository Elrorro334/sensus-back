using FluentAssertions;
using Sensus.Application.Comercios.Commands.UpdateMerchantProfile;
using Sensus.Application.Comercios.DTOs;

namespace Sensus.Application.UnitTests.Comercios.Commands;

public class UpdateMerchantProfileCommandValidatorTests
{
    private readonly UpdateMerchantProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new UpdateMerchantProfileCommand(
            "Mi Tiendita",
            "Av. Insurgentes Sur 1234, Benito Juárez",
            "03100",
            [
                new HorarioDiaDto(DayOfWeek.Monday, "09:00", "18:00", false),
                new HorarioDiaDto(DayOfWeek.Sunday, null, null, true)
            ]);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")] // Too short
    [InlineData("ABCDE")] // Non-numeric
    [InlineData("031001")] // 6 digits
    public void Validate_ShouldFail_WhenCodigoPostalIsNot5Digits(string invalidCp)
    {
        var command = new UpdateMerchantProfileCommand(
            "Mi Tiendita",
            "Av. Insurgentes Sur 1234",
            invalidCp,
            null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateMerchantProfileCommand.CodigoPostal));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Av")] // < 5 chars
    public void Validate_ShouldFail_WhenDireccionIsTooShortOrEmpty(string invalidDir)
    {
        var command = new UpdateMerchantProfileCommand(
            "Mi Tiendita",
            invalidDir,
            "03100",
            null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateMerchantProfileCommand.Direccion));
    }
}
