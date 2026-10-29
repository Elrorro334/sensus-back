using MediatR;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.RequestPasswordReset;

/// <summary>
/// SE-005: Solicitar enlace de recuperación de contraseña vía correo electrónico.
/// </summary>
public sealed record RequestPasswordResetCommand(string Correo) : IRequest<Result>;
