using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sensus.Application.Common.Behaviors;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Application.UnitTests.Common.Behaviors;

public class LoggingBehaviorTests
{
    private readonly ILogger<LoggingBehavior<SampleSensitiveRequest, string>> _sensitiveLogger =
        Substitute.For<ILogger<LoggingBehavior<SampleSensitiveRequest, string>>>();

    private readonly ILogger<LoggingBehavior<SampleNormalRequest, string>> _normalLogger =
        Substitute.For<ILogger<LoggingBehavior<SampleNormalRequest, string>>>();

    public sealed record SampleSensitiveRequest(string Password) : IRequest<string>, ISensitiveRequest;
    public sealed record SampleNormalRequest(string Query) : IRequest<string>;

    [Fact]
    public async Task Handle_ShouldRedactPayload_WhenRequestIsSensitive()
    {
        var behavior = new LoggingBehavior<SampleSensitiveRequest, string>(_sensitiveLogger);
        var request = new SampleSensitiveRequest("P@ssw0rd123!");
        RequestHandlerDelegate<string> next = (ct) => Task.FromResult("OK");

        var response = await behavior.Handle(request, next, CancellationToken.None);

        response.Should().Be("OK");
        _sensitiveLogger.Received().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("[DATOS REDACTADOS POR SEGURIDAD]") && !o.ToString()!.Contains("P@ssw0rd123!")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_ShouldLogPayload_WhenRequestIsNotSensitive()
    {
        var behavior = new LoggingBehavior<SampleNormalRequest, string>(_normalLogger);
        var request = new SampleNormalRequest("test-query");
        RequestHandlerDelegate<string> next = (ct) => Task.FromResult("OK");

        var response = await behavior.Handle(request, next, CancellationToken.None);

        response.Should().Be("OK");
        _normalLogger.Received().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString()!.Contains("SampleNormalRequest")),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }
}
