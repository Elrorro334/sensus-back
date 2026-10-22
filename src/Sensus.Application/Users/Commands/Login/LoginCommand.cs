using MediatR;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.Login;

/// <summary>
/// SE-002: Comando para iniciar sesión de forma segura con correo y contraseña.
/// </summary>
public sealed record LoginCommand(
    string Correo,
    string Password) : IRequest<Result<AuthResponseDto>>;
