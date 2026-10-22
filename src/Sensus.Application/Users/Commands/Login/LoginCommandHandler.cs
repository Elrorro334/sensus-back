using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Mensaje genérico de seguridad para no exponer si el correo existe o no
        var invalidCredentialsError = Error.Unauthorized(
            "Auth.InvalidCredentials",
            "Correo o contraseña incorrectos.");

        var user = await _userRepository.GetByEmailAsync(request.Correo, cancellationToken);
        if (user is null)
        {
            return Result.Failure<AuthResponseDto>(invalidCredentialsError);
        }

        if (!user.Estatus)
        {
            return Result.Failure<AuthResponseDto>(
                Error.Forbidden("Auth.UserDeactivated", "La cuenta se encuentra inactiva o suspendida."));
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Result.Failure<AuthResponseDto>(invalidCredentialsError);
        }

        // Registrar último acceso
        var now = _dateTimeProvider.UtcNow;
        user.RecordAccess(now);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generar token JWT con claims y roles
        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto(
            token,
            "Bearer",
            expiresAt,
            user.ToDto());

        return Result.Success(response);
    }
}
