using Sensus.Domain.Common;
using Sensus.Domain.Events;

namespace Sensus.Domain.Entities;

/// <summary>
/// Raíz de Agregado Comercio que representa el local comercial del usuario (SE-003 & SE-004).
/// </summary>
public class Comercio : BaseAuditableEntity, IAggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public string NombreComercial { get; private set; } = string.Empty;
    public string Direccion { get; private set; } = string.Empty;
    public string CodigoPostal { get; private set; } = string.Empty;
    public double Latitud { get; private set; }
    public double Longitud { get; private set; }
    public bool Activo { get; private set; }
    public DateTime FechaRegistro { get; private set; }

    // Constructor privado para serialización / ORM
    private Comercio() : base()
    {
    }

    private Comercio(
        Guid id,
        Guid usuarioId,
        string nombreComercial,
        string direccion,
        string codigoPostal,
        double latitud,
        double longitud,
        DateTime fechaRegistro) : base(id)
    {
        UsuarioId = usuarioId;
        NombreComercial = nombreComercial;
        Direccion = direccion;
        CodigoPostal = codigoPostal;
        Latitud = latitud;
        Longitud = longitud;
        Activo = true;
        FechaRegistro = fechaRegistro;
        CreatedAtUtc = fechaRegistro;
    }

    /// <summary>
    /// Factory method para registrar un nuevo comercio validando las invariantes de dominio.
    /// </summary>
    public static Result<Comercio> Create(
        Guid usuarioId,
        string nombreComercial,
        string direccion,
        string codigoPostal,
        double latitud,
        double longitud,
        DateTime? fechaRegistro = null)
    {
        if (usuarioId == Guid.Empty)
        {
            return Result.Failure<Comercio>(
                Error.Validation("Comercio.UsuarioInvalido", "El identificador del usuario no es válido."));
        }

        if (string.IsNullOrWhiteSpace(nombreComercial))
        {
            return Result.Failure<Comercio>(
                Error.Validation("Comercio.NombreComercialVacio", "El nombre comercial es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            return Result.Failure<Comercio>(
                Error.Validation("Comercio.DireccionVacia", "La dirección del comercio es obligatoria."));
        }

        if (string.IsNullOrWhiteSpace(codigoPostal))
        {
            return Result.Failure<Comercio>(
                Error.Validation("Comercio.CodigoPostalVacio", "El código postal es obligatorio."));
        }

        var registrationDate = fechaRegistro ?? DateTime.UtcNow;

        var comercio = new Comercio(
            Guid.NewGuid(),
            usuarioId,
            nombreComercial.Trim(),
            direccion.Trim(),
            codigoPostal.Trim(),
            latitud,
            longitud,
            registrationDate);

        comercio.AddDomainEvent(new ComercioRegisteredDomainEvent(comercio.Id, comercio.UsuarioId, comercio.NombreComercial));

        return Result.Success(comercio);
    }

    /// <summary>
    /// Actualiza los datos de ubicación e información comercial.
    /// </summary>
    public Result UpdateDetails(
        string nombreComercial,
        string direccion,
        string codigoPostal,
        double latitud,
        double longitud)
    {
        if (string.IsNullOrWhiteSpace(nombreComercial))
        {
            return Result.Failure(
                Error.Validation("Comercio.NombreComercialVacio", "El nombre comercial no puede estar vacío."));
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            return Result.Failure(
                Error.Validation("Comercio.DireccionVacia", "La dirección no puede estar vacía."));
        }

        if (string.IsNullOrWhiteSpace(codigoPostal))
        {
            return Result.Failure(
                Error.Validation("Comercio.CodigoPostalVacio", "El código postal no puede estar vacío."));
        }

        NombreComercial = nombreComercial.Trim();
        Direccion = direccion.Trim();
        CodigoPostal = codigoPostal.Trim();
        Latitud = latitud;
        Longitud = longitud;

        return Result.Success();
    }

    public void Deactivate()
    {
        Activo = false;
    }

    public void Activate()
    {
        Activo = true;
    }
}
