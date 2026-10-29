namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Abstracción para obtener el usuario autenticado actual desde el contexto de ejecución.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
    bool IsGuest { get; }
    bool IsMerchant { get; }
}
