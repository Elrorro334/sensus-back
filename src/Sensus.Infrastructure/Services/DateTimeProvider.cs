using Sensus.Application.Common.Interfaces;

namespace Sensus.Infrastructure.Services;

/// <summary>
/// Implementación del proveedor de fecha y hora basada en el reloj del sistema (UTC).
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
