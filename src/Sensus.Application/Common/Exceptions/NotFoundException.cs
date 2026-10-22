namespace Sensus.Application.Common.Exceptions;

/// <summary>
/// Excepción lanzada cuando una entidad requerida no fue encontrada.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"La entidad \"{name}\" con identificador ({key}) no fue encontrada.")
    {
    }
}
