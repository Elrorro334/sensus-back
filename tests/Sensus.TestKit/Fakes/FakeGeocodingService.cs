using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;

namespace Sensus.TestKit.Fakes;

public sealed class FakeGeocodingService : IGeocodingService
{
    private double _latitude = 19.432608;
    private double _longitude = -99.133209;
    private bool _shouldFail;

    public void SetupCoordinates(double lat, double lon)
    {
        _latitude = lat;
        _longitude = lon;
        _shouldFail = false;
    }

    public void SetupFailure()
    {
        _shouldFail = true;
    }

    public Task<Result<(double Latitude, double Longitude)>> GeocodeAddressAsync(
        string direccion,
        string codigoPostal,
        CancellationToken cancellationToken = default)
    {
        if (_shouldFail)
        {
            return Task.FromResult(Result.Failure<(double Latitude, double Longitude)>(
                Error.Unprocessable("Geocoding.NotFound", "No fue posible geocodificar la dirección especificada.")));
        }

        return Task.FromResult(Result.Success((_latitude, _longitude)));
    }
}
