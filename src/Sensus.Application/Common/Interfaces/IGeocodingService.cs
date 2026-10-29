using Sensus.Domain.Common;

namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Servicio para geocodificación de direcciones hacia coordenadas geográficas (latitud, longitud).
/// Devuelve un resultado explícito para evitar falsos positivos y caídas silenciosas a CDMX (SE-007).
/// </summary>
public interface IGeocodingService
{
    Task<Result<(double Latitude, double Longitude)>> GeocodeAddressAsync(
        string direccion,
        string codigoPostal,
        CancellationToken cancellationToken = default);
}
