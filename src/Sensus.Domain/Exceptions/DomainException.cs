namespace Sensus.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando se violan invariantes o reglas de negocio críticas en el dominio.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
