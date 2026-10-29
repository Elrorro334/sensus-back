using FluentAssertions;
using Sensus.Domain.Policies;

namespace Sensus.Application.UnitTests.Policies;

public class GuestRadiusPolicyTests
{
    [Theory]
    [InlineData(500, 500)]
    [InlineData(1999, 1999)]
    [InlineData(2000, 2000)]
    [InlineData(2001, 2000)] // Capped to 2000m
    [InlineData(5000, 2000)] // Capped to 2000m
    [InlineData(10000, 2000)] // Capped to 2000m
    [InlineData(0, 2000)] // Default to 2000m
    [InlineData(-100, 2000)] // Default to 2000m
    public void ResolveRadius_ShouldStrictlyEnforce2KmMax_ForGuest(int requested, int expected)
    {
        var resolved = GuestRadiusPolicy.ResolveRadius(isGuest: true, requestedRadiusMeters: requested);

        resolved.Should().Be(expected);
    }

    [Theory]
    [InlineData(500, 500)]
    [InlineData(2000, 2000)]
    [InlineData(5000, 5000)]
    [InlineData(10000, 10000)]
    [InlineData(50000, 50000)]
    [InlineData(60000, 50000)] // Capped at registered max 50km
    public void ResolveRadius_ShouldAllowHigherRadii_ForRegisteredUser(int requested, int expected)
    {
        var resolved = GuestRadiusPolicy.ResolveRadius(isGuest: false, requestedRadiusMeters: requested);

        resolved.Should().Be(expected);
    }
}
