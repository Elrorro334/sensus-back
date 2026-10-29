using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Infrastructure.Services;

/// <summary>
/// Servicio de geocodificación mediante OpenStreetMap Nominatim con resiliencia y fallback para pruebas offline (SE-003).
/// </summary>
public sealed class NominatimGeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NominatimGeocodingService> _logger;

    // Coordenadas de referencia por defecto (Ciudad de México) para fallback
    private const double DefaultLatitude = 19.432608;
    private const double DefaultLongitude = -99.133209;

    public NominatimGeocodingService(
        ILogger<NominatimGeocodingService> logger,
        HttpClient? httpClient = null)
    {
        _logger = logger;
        _httpClient = httpClient ?? new HttpClient();

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
        }

        if (!_httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Sensus-App/1.0 (sensus-backend@enterprise.local)");
        }
    }

    public async Task<(double Latitude, double Longitude)> GeocodeAddressAsync(
        string direccion,
        string codigoPostal,
        CancellationToken cancellationToken = default)
    {
        var searchQuery = $"{direccion}, {codigoPostal}";

        try
        {
            var url = $"search?q={Uri.EscapeDataString(searchQuery)}&format=json&limit=1";
            var response = await _httpClient.GetFromJsonAsync<List<NominatimResult>>(url, cancellationToken);

            if (response is { Count: > 0 })
            {
                var first = response[0];
                if (double.TryParse(first.Lat, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) &&
                    double.TryParse(first.Lon, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                {
                    _logger.LogInformation(
                        "Dirección '{Query}' geocodificada exitosamente: ({Lat}, {Lon})",
                        searchQuery, lat, lon);

                    return (lat, lon);
                }
            }

            _logger.LogWarning(
                "Nominatim no encontró resultados para '{Query}'. Asignando coordenadas de referencia.",
                searchQuery);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Error al conectar con el servicio de geocodificación externa para '{Query}'. Usando coordenadas de respaldo.",
                searchQuery);
        }

        return (DefaultLatitude, DefaultLongitude);
    }

    private sealed class NominatimResult
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = string.Empty;

        [JsonPropertyName("lon")]
        public string Lon { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
