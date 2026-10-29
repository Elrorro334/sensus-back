using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.DeleteAccount;

/// <summary>
/// SE-009: Eliminación de cuenta y datos del usuario (exige confirmación con contraseña actual).
/// </summary>
public sealed record DeleteAccountCommand(string Password) : IRequest<Result>, ISensitiveRequest;
