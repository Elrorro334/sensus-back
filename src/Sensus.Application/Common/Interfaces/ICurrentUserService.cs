namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Abstracción para obtener el usuario autenticado actual desde el contexto de ejecución.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
}
