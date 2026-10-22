using Sensus.Domain.Common;
using Sensus.Domain.Enums;
using Sensus.Domain.Events;

namespace Sensus.Domain.Entities;

/// <summary>
/// Raíz de Agregado Usuario que implementa el Modelo Unificado (Airbnb) para Consumidor y Comerciante.
/// </summary>
public class User : BaseEntity, IAggregateRoot
{
    private readonly List<string> _roles = [];

    public string NombreCompleto { get; private set; } = string.Empty;
    public string Correo { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool AceptoTerminos { get; private set; }
    public DateTime? FechaAceptacionTerminos { get; private set; }
    public bool Estatus { get; private set; }
    public DateTime FechaRegistro { get; private set; }
    public DateTime? UltimoAcceso { get; private set; }

    public IReadOnlyCollection<string> Roles => _roles.AsReadOnly();

    // Constructor privado para serialización / ORM
    private User() : base()
    {
    }

    private User(
        Guid id,
        string nombreCompleto,
        string correo,
        string passwordHash,
        bool aceptoTerminos,
        DateTime fechaRegistro) : base(id)
    {
        NombreCompleto = nombreCompleto;
        Correo = correo;
        PasswordHash = passwordHash;
        AceptoTerminos = aceptoTerminos;
        FechaAceptacionTerminos = aceptoTerminos ? fechaRegistro : null;
        Estatus = true; // Activo por defecto
        FechaRegistro = fechaRegistro;
        _roles.Add(RoleType.Consumidor); // Rol unificado base
    }

    /// <summary>
    /// Factory method para registrar un nuevo usuario en el sistema.
    /// </summary>
    public static Result<User> Create(
        string nombreCompleto,
        string correo,
        string passwordHash,
        bool aceptoTerminos,
        DateTime fechaRegistro)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto))
        {
            return Result.Failure<User>(
                Error.Validation("User.NombreVacio", "El nombre completo es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(correo))
        {
            return Result.Failure<User>(
                Error.Validation("User.CorreoVacio", "El correo electrónico es obligatorio."));
        }

        if (!aceptoTerminos)
        {
            return Result.Failure<User>(
                Error.Validation("User.TerminosRequeridos", "Debe aceptar los términos y condiciones para registrarse."));
        }

        var normalizedEmail = correo.Trim().ToLowerInvariant();
        var user = new User(
            Guid.NewGuid(),
            nombreCompleto.Trim(),
            normalizedEmail,
            passwordHash,
            aceptoTerminos,
            fechaRegistro);

        user.AddDomainEvent(new UserRegisteredDomainEvent(user.Id, user.Correo, user.NombreCompleto));

        return Result.Success(user);
    }

    /// <summary>
    /// Agrega un rol al usuario para el modelo unificado (ej. Comerciante en SE-003).
    /// </summary>
    public Result AddRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return Result.Failure(Error.Validation("Role.Empty", "El nombre del rol no puede estar vacío."));
        }

        if (!_roles.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            _roles.Add(role);
        }

        return Result.Success();
    }

    /// <summary>
    /// Comprueba si el usuario tiene asignado un rol específico.
    /// </summary>
    public bool HasRole(string role) =>
        _roles.Contains(role, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registra la fecha y hora del último acceso exitoso del usuario.
    /// </summary>
    public void RecordAccess(DateTime timestampUtc)
    {
        UltimoAcceso = timestampUtc;
        AddDomainEvent(new UserLoggedInDomainEvent(Id, timestampUtc));
    }

    /// <summary>
    /// Actualiza el hash de contraseña en caso de recuperación o cambio.
    /// </summary>
    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    /// <summary>
    /// Desactiva la cuenta del usuario.
    /// </summary>
    public void Deactivate()
    {
        Estatus = false;
    }

    /// <summary>
    /// Activa la cuenta del usuario.
    /// </summary>
    public void Activate()
    {
        Estatus = true;
    }
}
