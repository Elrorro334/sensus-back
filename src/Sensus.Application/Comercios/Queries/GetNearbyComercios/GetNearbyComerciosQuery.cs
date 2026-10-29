using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Policies;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Comercios.Queries.GetNearbyComercios;

/// <summary>
/// SE-008: Consulta comercios cercanos al usuario en base a coordenadas geográficas.
/// Aplica en servidor la restricción estricta de 2 km para sesiones de invitado.
/// </summary>
public sealed record GetNearbyComerciosQuery(
    double Latitud,
    double Longitud,
    int? RadioMetros = null) : IRequest<Result<IReadOnlyList<ComercioDto>>>;

public sealed class GetNearbyComerciosQueryHandler : IRequestHandler<GetNearbyComerciosQuery, Result<IReadOnlyList<ComercioDto>>>
{
    private readonly IComercioRepository _comercioRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetNearbyComerciosQueryHandler(
        IComercioRepository comercioRepository,
        ICurrentUserService currentUserService)
    {
        _comercioRepository = comercioRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IReadOnlyList<ComercioDto>>> Handle(GetNearbyComerciosQuery request, CancellationToken cancellationToken)
    {
        var isGuest = _currentUserService.IsGuest;
        var requestedRadius = request.RadioMetros ?? GuestRadiusPolicy.DefaultRadiusMeters;
        var effectiveRadius = GuestRadiusPolicy.ResolveRadius(isGuest, requestedRadius);

        var comercios = await _comercioRepository.GetNearbyAsync(
            request.Latitud,
            request.Longitud,
            effectiveRadius,
            cancellationToken);

        var dtos = comercios.Select(c => c.ToDto()).ToList();

        return Result.Success<IReadOnlyList<ComercioDto>>(dtos);
    }
}
