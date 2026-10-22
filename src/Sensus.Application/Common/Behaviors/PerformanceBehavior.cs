using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sensus.Application.Common.Behaviors;

/// <summary>
/// Pipeline Behavior para detectar y advertir sobre operaciones lentas en la aplicación.
/// </summary>
public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private const int WarningThresholdMs = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next(cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > WarningThresholdMs)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning(
                "Alerta de rendimiento en solicitud de larga duración: {RequestName} tomó {ElapsedMilliseconds} ms. {@Request}",
                requestName,
                elapsedMilliseconds,
                request);
        }

        return response;
    }
}
