using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterUserCommandHandler(
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
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(request.Correo, cancellationToken);
        if (emailExists)
        {
            return Result.Failure<AuthResponseDto>(
                Error.Conflict("User.EmailExists", "El correo electrónico ya se encuentra registrado."));
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var now = _dateTimeProvider.UtcNow;

        var createResult = User.Create(
            request.NombreCompleto,
            request.Correo,
            passwordHash,
            request.AceptoTerminos,
            now);

        if (createResult.IsFailure)
        {
            return Result.Failure<AuthResponseDto>(createResult.Error);
        }

        var user = createResult.Value;

        await _userRepository.AddAsync(user, cancellationToken);

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto(
            token,
            "Bearer",
            expiresAt,
            user.ToDto());

        return Result.Success(response);
    }
}
