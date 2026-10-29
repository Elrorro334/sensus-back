using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.ResetPassword;

/// <summary>
/// SE-005: Restablecer contraseña utilizando el token de verificación recibido por correo.
/// </summary>
public sealed record ResetPasswordCommand(
    string Token,
    string NuevaPassword) : IRequest<Result>, ISensitiveRequest;
