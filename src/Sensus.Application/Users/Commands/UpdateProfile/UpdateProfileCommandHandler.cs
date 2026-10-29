using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UserDto>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var rawUserId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(rawUserId) || !Guid.TryParse(rawUserId, out var userId))
        {
            return Result.Failure<UserDto>(Error.Unauthorized("Auth.Unauthorized", "Se requiere sesión activa para editar el perfil."));
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null || !user.Estatus)
        {
            return Result.Failure<UserDto>(Error.NotFound("User.NotFound", "El usuario no existe o se encuentra inactivo."));
        }

        var updateResult = user.UpdateProfile(request.NombreCompleto, request.FotoPerfilUrl);
        if (updateResult.IsFailure)
        {
            return Result.Failure<UserDto>(updateResult.Error);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success(user.ToDto());
    }
}
