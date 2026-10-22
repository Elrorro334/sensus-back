namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Contrato para el hasheo y verificación segura de contraseñas (BCrypt).
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
