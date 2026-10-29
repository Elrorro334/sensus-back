using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.RequestPasswordReset;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Users.Commands;

public class RequestPasswordResetCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordResetTokenRepository _tokenRepository = Substitute.For<IPasswordResetTokenRepository>();
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ILogger<RequestPasswordResetCommandHandler> _logger = Substitute.For<ILogger<RequestPasswordResetCommandHandler>>();

    private readonly RequestPasswordResetCommandHandler _handler;

    public RequestPasswordResetCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(new DateTime(2026, 10, 29, 12, 0, 0, DateTimeKind.Utc));
        _handler = new RequestPasswordResetCommandHandler(
            _userRepository,
            _tokenRepository,
            _emailSender,
            _dateTimeProvider,
            _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_EvenIfEmailDoesNotExist_AntiEnumeration()
    {
        _userRepository.GetByEmailAsync("nonexistent@sensus.com", Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var command = new RequestPasswordResetCommand("nonexistent@sensus.com");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _emailSender.DidNotReceive().SendEmailAsync(
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldGenerateTokenAndSendEmail_WhenUserExists()
    {
        var user = User.Create("Oscar", "oscar@sensus.com", "hash", true, DateTime.UtcNow).Value;
        _userRepository.GetByEmailAsync("oscar@sensus.com", Arg.Any<CancellationToken>())
            .Returns(user);

        var command = new RequestPasswordResetCommand("oscar@sensus.com");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _tokenRepository.Received(1).InvalidateExistingTokensForUserAsync(user.Id, Arg.Any<CancellationToken>());
        await _tokenRepository.Received(1).AddAsync(Arg.Any<PasswordResetToken>(), Arg.Any<CancellationToken>());
        await _emailSender.Received(1).SendEmailAsync(
            user.Correo,
            Arg.Is<string>(s => s.Contains("contraseña")),
            Arg.Is<string>(html => html.Contains("15 minutos")),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
