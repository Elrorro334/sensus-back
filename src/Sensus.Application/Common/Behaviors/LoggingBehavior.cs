using MediatR;
using Microsoft.Extensions.Logging;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Application.Common.Behaviors;

/// <summary>
/// Pipeline Behavior para logging estructurado y seguro de comandos y consultas.
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        if (request is ISensitiveRequest)
        {
            _logger.LogInformation("Iniciando solicitud confidencial: {RequestName} [DATOS REDACTADOS POR SEGURIDAD]", requestName);
        }
        else
        {
            _logger.LogInformation("Iniciando solicitud: {RequestName} {@Request}", requestName, request);
        }

        try
        {
            var response = await next(cancellationToken);
            _logger.LogInformation("Solicitud completada exitosamente: {RequestName}", requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado al procesar la solicitud: {RequestName}", requestName);
            throw;
        }
    }
}
