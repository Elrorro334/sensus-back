using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var nowUtc = _dateTimeProvider.UtcNow;
        var tokenHash = PasswordResetToken.ComputeHash(request.Token.Trim());

        var token = await _tokenRepository.GetByTokenHashAsync(tokenHash, cancellationToken);
        if (token is null || !token.IsValid(nowUtc))
        {
            return Result.Failure(Error.Validation(
                "Auth.InvalidResetToken",
                "El enlace de recuperación es inválido, ha expirado o ya fue utilizado."));
        }

        // Consumo atómico del token para evitar reuso concurrente
        var consumed = await _tokenRepository.TryConsumeAsync(tokenHash, nowUtc, cancellationToken);
        if (!consumed)
        {
            return Result.Failure(Error.Validation(
                "Auth.InvalidResetToken",
                "El enlace de recuperación ya ha sido consumido."));
        }

        var user = await _userRepository.GetByIdAsync(token.UserId, cancellationToken);
        if (user is null || !user.Estatus)
        {
            return Result.Failure(Error.NotFound("User.NotFound", "El usuario no existe o se encuentra inactivo."));
        }

        // Actualizar contraseña y rotar SecurityStamp para invalidar sesiones existentes
        var newHash = _passwordHasher.HashPassword(request.NuevaPassword);
        user.UpdatePassword(newHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
