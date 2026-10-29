using MediatR;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Comercios.Commands.RegisterComercio;

/// <summary>
/// Handler que orquesta el registro del local (SE-003) y la activación dinámica del rol comercial (SE-004).
/// </summary>
public sealed class RegisterComercioCommandHandler : IRequestHandler<RegisterComercioCommand, Result<RegisterComercioResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IComercioRepository _comercioRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGeocodingService _geocodingService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterComercioCommandHandler(
        IUserRepository userRepository,
        IComercioRepository comercioRepository,
        ICurrentUserService currentUserService,
        IGeocodingService geocodingService,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _comercioRepository = comercioRepository;
        _currentUserService = currentUserService;
        _geocodingService = geocodingService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<RegisterComercioResponseDto>> Handle(
        RegisterComercioCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || !Guid.TryParse(_currentUserService.UserId, out var usuarioId))
        {
            return Result.Failure<RegisterComercioResponseDto>(
                Error.Unauthorized("Auth.Unauthorized", "Debe iniciar sesión para registrar un comercio."));
        }

        var user = await _userRepository.GetByIdAsync(usuarioId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<RegisterComercioResponseDto>(
                Error.NotFound("User.NotFound", "No se encontró el usuario especificado."));
        }

        var alreadyHasStore = await _comercioRepository.ExistsByUsuarioIdAsync(usuarioId, cancellationToken);
        if (alreadyHasStore)
        {
            return Result.Failure<RegisterComercioResponseDto>(
                Error.Conflict("Comercio.AlreadyExists", "El usuario ya posee un comercio activo registrado."));
        }

        // Geocodificación (SE-003: Latitud/Longitud para GeoJSON)
        var (latitud, longitud) = await _geocodingService.GeocodeAddressAsync(
            request.Direccion,
            request.CodigoPostal,
            cancellationToken);

        var now = _dateTimeProvider.UtcNow;
        var comercioResult = Comercio.Create(
            usuarioId,
            request.NombreComercial,
            request.Direccion,
            request.CodigoPostal,
            latitud,
            longitud,
            now);

        if (comercioResult.IsFailure)
        {
            return Result.Failure<RegisterComercioResponseDto>(comercioResult.Error);
        }

        var comercio = comercioResult.Value;
        await _comercioRepository.AddAsync(comercio, cancellationToken);

        // Actualización dinámica de rol en base de datos (SE-004)
        var addRoleResult = user.AddRole(RoleType.Comerciante);
        if (addRoleResult.IsFailure)
        {
            return Result.Failure<RegisterComercioResponseDto>(addRoleResult.Error);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generar nuevo token con rol 'Comerciante' para permitir conmutación de vista sin re-login
        var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateToken(user);

        var response = new RegisterComercioResponseDto(
            Mensaje: "Comercio registrado y rol comercial activado exitosamente.",
            ComercioId: comercio.Id,
            EsComerciante: true,
            Roles: user.Roles.ToList(),
            Comercio: comercio.ToDto(),
            Token: token,
            TokenType: "Bearer",
            ExpiresAtUtc: expiresAtUtc);

        return Result.Success(response);
    }
}
