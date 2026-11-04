using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.Logout;

/// <summary>
/// Manejador para el cierre de sesión que rota el SecurityStamp del usuario,
/// invalidando inmediatamente todos los tokens JWT emitidos previamente (FR-004-01 / FR-004-04).
/// </summary>
public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public LogoutCommandHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var rawUserId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(rawUserId) || !Guid.TryParse(rawUserId, out var userId))
        {
            return Result.Failure(Error.Unauthorized("Auth.Unauthorized", "Se requiere una sesión activa para cerrar sesión."));
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            // Idempotencia: si el usuario ya no existe, la sesión ya no tiene efecto
            return Result.Success();
        }

        user.RotateSecurityStamp();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
