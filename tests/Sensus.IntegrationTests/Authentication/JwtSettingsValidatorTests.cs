using FluentAssertions;
using Microsoft.Extensions.Options;
using Sensus.Infrastructure.Authentication;

namespace Sensus.IntegrationTests.Authentication;

public class JwtSettingsValidatorTests
{
    private readonly JwtSettingsValidator _validator = new();

    [Fact]
    public void Validate_ShouldFail_WhenSecretIsTooShort()
    {
        var settings = new JwtSettings
        {
            Secret = "short-secret",
            Issuer = "SensusApi",
            Audience = "SensusApp",
            ExpiryMinutes = 60
        };

        var result = _validator.Validate(null, settings);

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("al menos 32 caracteres");
    }

    [Theory]
    [InlineData("SensusSuperSecretKey2026_Minimum32CharactersLong!")]
    [InlineData("SensusSuperSecretKey2026_Minimum32CharactersLong_Enterprise!")]
    public void Validate_ShouldFail_WhenSecretIsKnownLeakedHistoricalValue(string leakedSecret)
    {
        var settings = new JwtSettings
        {
            Secret = leakedSecret,
            Issuer = "SensusApi",
            Audience = "SensusApp",
            ExpiryMinutes = 60
        };

        var result = _validator.Validate(null, settings);

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("histórico o filtrado");
    }

    [Fact]
    public void Validate_ShouldSucceed_WhenSettingsAreValidAndStrong()
    {
        var settings = new JwtSettings
        {
            Secret = "A_Very_Strong_And_Rotated_Secret_Key_For_Sensus_Production_2026!",
            Issuer = "SensusApi",
            Audience = "SensusApp",
            ExpiryMinutes = 1440
        };

        var result = _validator.Validate(null, settings);

        result.Succeeded.Should().BeTrue();
    }
}
