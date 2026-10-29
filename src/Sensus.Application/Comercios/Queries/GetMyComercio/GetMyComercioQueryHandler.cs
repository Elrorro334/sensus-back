using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Comercios.Queries.GetMyComercio;

/// <summary>
/// Handler para consultar el comercio propio del usuario para habilitar el panel comercial (SE-004).
/// </summary>
public sealed class GetMyComercioQueryHandler : IRequestHandler<GetMyComercioQuery, Result<ComercioDto>>
{
    private readonly IComercioRepository _comercioRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyComercioQueryHandler(
        IComercioRepository comercioRepository,
        ICurrentUserService currentUserService)
    {
        _comercioRepository = comercioRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ComercioDto>> Handle(
        GetMyComercioQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || !Guid.TryParse(_currentUserService.UserId, out var usuarioId))
        {
            return Result.Failure<ComercioDto>(
                Error.Unauthorized("Auth.Unauthorized", "Debe iniciar sesión para consultar los datos de su comercio."));
        }

        var comercio = await _comercioRepository.GetByUsuarioIdAsync(usuarioId, cancellationToken);
        if (comercio is null)
        {
            return Result.Failure<ComercioDto>(
                Error.NotFound("Comercio.NotFound", "El usuario no tiene ningún comercio registrado."));
        }

        return Result.Success(comercio.ToDto());
    }
}
