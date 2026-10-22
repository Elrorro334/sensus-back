using MediatR;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.RegisterUser;

/// <summary>
/// SE-001: Comando para registrar un nuevo usuario con correo y contraseña bajo el modelo unificado.
/// </summary>
public sealed record RegisterUserCommand(
    string NombreCompleto,
    string Correo,
    string Password,
    bool AceptoTerminos) : IRequest<Result<AuthResponseDto>>;
