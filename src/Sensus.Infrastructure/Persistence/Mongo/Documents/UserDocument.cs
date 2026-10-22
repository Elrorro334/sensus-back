using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sensus.Domain.Entities;

namespace Sensus.Infrastructure.Persistence.Mongo.Documents;

/// <summary>
/// Documento de usuario en MongoDB Atlas migrado desde el modelo SQL (Usuarios, Roles, UsuarioRoles).
/// </summary>
public sealed class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("nombreCompleto")]
    public string NombreCompleto { get; set; } = string.Empty;

    [BsonElement("correo")]
    public string Correo { get; set; } = string.Empty;

    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("aceptoTerminos")]
    public bool AceptoTerminos { get; set; }

    [BsonElement("fechaAceptacionTerminos")]
    public DateTime? FechaAceptacionTerminos { get; set; }

    [BsonElement("estatus")]
    public bool Estatus { get; set; }

    [BsonElement("fechaRegistro")]
    public DateTime FechaRegistro { get; set; }

    [BsonElement("ultimoAcceso")]
    public DateTime? UltimoAcceso { get; set; }

    [BsonElement("roles")]
    public List<string> Roles { get; set; } = [];

    public static UserDocument FromDomain(User user)
    {
        return new UserDocument
        {
            Id = user.Id,
            NombreCompleto = user.NombreCompleto,
            Correo = user.Correo,
            PasswordHash = user.PasswordHash,
            AceptoTerminos = user.AceptoTerminos,
            FechaAceptacionTerminos = user.FechaAceptacionTerminos,
            Estatus = user.Estatus,
            FechaRegistro = user.FechaRegistro,
            UltimoAcceso = user.UltimoAcceso,
            Roles = user.Roles.ToList()
        };
    }

    public User ToDomain()
    {
        var user = User.Create(
            NombreCompleto,
            Correo,
            PasswordHash,
            AceptoTerminos,
            FechaRegistro).Value;

        // Establecer el Id persistido y estado
        typeof(User).GetProperty("Id")?.SetValue(user, Id);

        if (!Estatus)
        {
            user.Deactivate();
        }

        if (UltimoAcceso.HasValue)
        {
            user.RecordAccess(UltimoAcceso.Value);
        }

        // Agregar roles adicionales del modelo unificado si existen
        foreach (var role in Roles)
        {
            if (!user.HasRole(role))
            {
                user.AddRole(role);
            }
        }

        user.ClearDomainEvents();
        return user;
    }
}
