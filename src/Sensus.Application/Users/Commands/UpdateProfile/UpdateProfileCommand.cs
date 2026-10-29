using MediatR;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.UpdateProfile;

/// <summary>
/// SE-006: Actualizar información personal (nombre y/o foto de perfil) del usuario autenticado.
/// </summary>
public sealed record UpdateProfileCommand(
    string NombreCompleto,
    string? FotoPerfilUrl) : IRequest<Result<UserDto>>;
