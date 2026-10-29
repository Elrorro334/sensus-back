namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Marcador para solicitudes (comandos o consultas) que contienen datos confidenciales o sensibles (ej. contraseñas, tokens).
/// El Pipeline de logging redacta el cuerpo de estas solicitudes para evitar fugas de información confidencial.
/// </summary>
public interface ISensitiveRequest
{
}
