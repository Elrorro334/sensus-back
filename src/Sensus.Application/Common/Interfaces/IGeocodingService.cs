namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Servicio para geocodificación de direcciones hacia coordenadas geográficas (latitud, longitud).
/// </summary>
public interface IGeocodingService
{
    Task<(double Latitude, double Longitude)> GeocodeAddressAsync(
        string direccion,
        string codigoPostal,
        CancellationToken cancellationToken = default);
}
