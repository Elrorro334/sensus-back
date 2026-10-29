using FluentAssertions;
using Sensus.Application.Users.Commands.DeleteAccount;

namespace Sensus.Application.UnitTests.Users.Commands;

public class DeleteAccountCommandValidatorTests
{
    private readonly DeleteAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenPasswordIsProvided()
    {
        var command = new DeleteAccountCommand("MiPasswordSegura123#");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_ShouldFail_WhenPasswordIsEmpty(string emptyPassword)
    {
        var command = new DeleteAccountCommand(emptyPassword);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(DeleteAccountCommand.Password));
    }
}
