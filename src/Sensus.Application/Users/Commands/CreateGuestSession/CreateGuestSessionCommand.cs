using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;
using Sensus.Domain.Enums;
using Sensus.Domain.Policies;

namespace Sensus.Application.Users.Commands.CreateGuestSession;

/// <summary>
/// SE-008: Iniciar sesión en modo invitado con radio restringido a 2 km.
/// </summary>
public sealed record CreateGuestSessionCommand : IRequest<Result<GuestSessionResponseDto>>;

public sealed class CreateGuestSessionCommandHandler : IRequestHandler<CreateGuestSessionCommand, Result<GuestSessionResponseDto>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public CreateGuestSessionCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public Task<Result<GuestSessionResponseDto>> Handle(CreateGuestSessionCommand request, CancellationToken cancellationToken)
    {
        var (token, expiresAtUtc) = _jwtTokenGenerator.GenerateGuestToken();

        var response = new GuestSessionResponseDto(
            Token: token,
            TokenType: "Bearer",
            ExpiresAtUtc: expiresAtUtc,
            MaxRadiusMeters: GuestRadiusPolicy.GuestMaxRadiusMeters,
            Role: RoleType.Invitado);

        return Task.FromResult(Result.Success(response));
    }
}
