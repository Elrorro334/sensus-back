using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using Sensus.Domain.Entities;

namespace Sensus.Infrastructure.Persistence.Mongo.Documents;

/// <summary>
/// Documento de comercio en MongoDB Atlas con soporte para coordenadas GeoJSON (SE-003 & SE-004).
/// </summary>
public sealed class ComercioDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    [BsonElement("usuarioId")]
    public Guid UsuarioId { get; set; }

    [BsonElement("nombreComercial")]
    public string NombreComercial { get; set; } = string.Empty;

    [BsonElement("direccion")]
    public string Direccion { get; set; } = string.Empty;

    [BsonElement("codigoPostal")]
    public string CodigoPostal { get; set; } = string.Empty;

    [BsonElement("latitud")]
    public double Latitud { get; set; }

    [BsonElement("longitud")]
    public double Longitud { get; set; }

    [BsonElement("ubicacion")]
    public GeoJsonPoint<GeoJson2DGeographicCoordinates>? Ubicacion { get; set; }

    [BsonElement("activo")]
    public bool Activo { get; set; }

    [BsonElement("fechaRegistro")]
    public DateTime FechaRegistro { get; set; }

    public static ComercioDocument FromDomain(Comercio comercio)
    {
        return new ComercioDocument
        {
            Id = comercio.Id,
            UsuarioId = comercio.UsuarioId,
            NombreComercial = comercio.NombreComercial,
            Direccion = comercio.Direccion,
            CodigoPostal = comercio.CodigoPostal,
            Latitud = comercio.Latitud,
            Longitud = comercio.Longitud,
            Ubicacion = GeoJson.Point(GeoJson.Geographic(comercio.Longitud, comercio.Latitud)),
            Activo = comercio.Activo,
            FechaRegistro = comercio.FechaRegistro
        };
    }

    public Comercio ToDomain()
    {
        var comercio = Comercio.Create(
            UsuarioId,
            NombreComercial,
            Direccion,
            CodigoPostal,
            Latitud,
            Longitud,
            FechaRegistro).Value;

        typeof(Comercio).GetProperty("Id")?.SetValue(comercio, Id);

        if (!Activo)
        {
            comercio.Deactivate();
        }

        comercio.ClearDomainEvents();
        return comercio;
    }
}
