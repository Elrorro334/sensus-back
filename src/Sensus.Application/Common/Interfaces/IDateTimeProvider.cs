namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Abstracción para el proveedor de fecha y hora, permitiendo pruebas deterministas en pruebas unitarias.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
