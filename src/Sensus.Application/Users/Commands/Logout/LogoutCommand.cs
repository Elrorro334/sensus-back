using MediatR;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.Logout;

/// <summary>
/// Comando CQRS para cerrar la sesión del usuario autenticado (FR-004-01).
/// </summary>
public sealed record LogoutCommand : IRequest<Result>;
