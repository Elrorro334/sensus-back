using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;
using Sensus.Domain.ValueObjects;

namespace Sensus.Application.Comercios.Commands.UpdateMerchantProfile;

public sealed class UpdateMerchantProfileCommandHandler : IRequestHandler<UpdateMerchantProfileCommand, Result<ComercioDto>>
{
    private readonly IComercioRepository _comercioRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGeocodingService _geocodingService;

    public UpdateMerchantProfileCommandHandler(
        IComercioRepository comercioRepository,
        ICurrentUserService currentUserService,
        IGeocodingService geocodingService)
    {
        _comercioRepository = comercioRepository;
        _currentUserService = currentUserService;
        _geocodingService = geocodingService;
    }

    public async Task<Result<ComercioDto>> Handle(UpdateMerchantProfileCommand request, CancellationToken cancellationToken)
    {
        var rawUserId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(rawUserId) || !Guid.TryParse(rawUserId, out var usuarioId))
        {
            return Result.Failure<ComercioDto>(
                Error.Unauthorized("Auth.Unauthorized", "Debe iniciar sesión para actualizar el local comercial."));
        }

        var comercio = await _comercioRepository.GetByUsuarioIdAsync(usuarioId, cancellationToken);
        if (comercio is null)
        {
            return Result.Failure<ComercioDto>(
                Error.NotFound("Comercio.NotFound", "No se encontró ningún comercio asociado al usuario actual."));
        }

        var nombreComercial = string.IsNullOrWhiteSpace(request.NombreComercial)
            ? comercio.NombreComercial
            : request.NombreComercial.Trim();

        var latitud = comercio.Latitud;
        var longitud = comercio.Longitud;

        // Solo se recalcula geocodificación si la ubicación física cambió (SE-007)
        if (comercio.HasPhysicalLocationChanged(request.Direccion, request.CodigoPostal))
        {
            var geocodeResult = await _geocodingService.GeocodeAddressAsync(
                request.Direccion,
                request.CodigoPostal,
                cancellationToken);

            if (geocodeResult.IsFailure)
            {
                return Result.Failure<ComercioDto>(geocodeResult.Error);
            }

            latitud = geocodeResult.Value.Latitude;
            longitud = geocodeResult.Value.Longitude;
        }

        var updateResult = comercio.UpdateDetails(
            nombreComercial,
            request.Direccion,
            request.CodigoPostal,
            latitud,
            longitud);

        if (updateResult.IsFailure)
        {
            return Result.Failure<ComercioDto>(updateResult.Error);
        }

        // Actualizar horarios de atención si se proporcionaron
        if (request.Horarios is not null)
        {
            var domainHorarios = new List<HorarioDia>();
            foreach (var h in request.Horarios)
            {
                if (h.EstaCerrado)
                {
                    domainHorarios.Add(HorarioDia.Cerrado(h.Dia).Value);
                }
                else
                {
                    if (!TimeOnly.TryParse(h.HoraApertura, out var apertura) ||
                        !TimeOnly.TryParse(h.HoraCierre, out var cierre))
                    {
                        return Result.Failure<ComercioDto>(
                            Error.Validation("Horario.FormatoInvalido", $"El formato de hora para el día {h.Dia} debe ser HH:mm."));
                    }

                    var horarioResult = HorarioDia.Abierto(h.Dia, apertura, cierre);
                    if (horarioResult.IsFailure)
                    {
                        return Result.Failure<ComercioDto>(horarioResult.Error);
                    }
                    domainHorarios.Add(horarioResult.Value);
                }
            }

            var horariosResult = comercio.UpdateHorarios(domainHorarios);
            if (horariosResult.IsFailure)
            {
                return Result.Failure<ComercioDto>(horariosResult.Error);
            }
        }

        await _comercioRepository.UpdateAsync(comercio, cancellationToken);

        return Result.Success(comercio.ToDto());
    }
}
