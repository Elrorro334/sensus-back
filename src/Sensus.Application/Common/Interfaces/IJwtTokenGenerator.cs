using Sensus.Domain.Entities;

namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Contrato para la generación de tokens de sesión JWT con claims de usuario, roles y modo invitado.
/// </summary>
public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
    (string Token, DateTime ExpiresAtUtc) GenerateGuestToken(Guid? guestId = null, int? expiryMinutes = null);
}
