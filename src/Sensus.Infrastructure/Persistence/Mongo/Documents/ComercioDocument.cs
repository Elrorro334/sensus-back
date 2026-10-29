using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using Sensus.Domain.Entities;
using Sensus.Domain.ValueObjects;

namespace Sensus.Infrastructure.Persistence.Mongo.Documents;

public sealed class HorarioDiaDocument
{
    [BsonElement("dia")]
    public DayOfWeek Dia { get; set; }

    [BsonElement("horaApertura")]
    public string? HoraApertura { get; set; }

    [BsonElement("horaCierre")]
    public string? HoraCierre { get; set; }

    [BsonElement("estaCerrado")]
    public bool EstaCerrado { get; set; }
}

/// <summary>
/// Documento de comercio en MongoDB Atlas con soporte para coordenadas GeoJSON y horarios (SE-003, SE-004 y SE-007).
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

    [BsonElement("horarios")]
    public List<HorarioDiaDocument> Horarios { get; set; } = [];

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
            FechaRegistro = comercio.FechaRegistro,
            Horarios = comercio.Horarios.Select(h => new HorarioDiaDocument
            {
                Dia = h.Dia,
                HoraApertura = h.HoraApertura?.ToString("HH:mm", CultureInfo.InvariantCulture),
                HoraCierre = h.HoraCierre?.ToString("HH:mm", CultureInfo.InvariantCulture),
                EstaCerrado = h.EstaCerrado
            }).ToList()
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

        if (Horarios.Count > 0)
        {
            var domainHorarios = new List<HorarioDia>();
            foreach (var h in Horarios)
            {
                if (h.EstaCerrado)
                {
                    domainHorarios.Add(HorarioDia.Cerrado(h.Dia).Value);
                }
                else if (TimeOnly.TryParse(h.HoraApertura, CultureInfo.InvariantCulture, out var ap) &&
                         TimeOnly.TryParse(h.HoraCierre, CultureInfo.InvariantCulture, out var ci))
                {
                    var res = HorarioDia.Abierto(h.Dia, ap, ci);
                    if (res.IsSuccess)
                    {
                        domainHorarios.Add(res.Value);
                    }
                }
            }
            comercio.UpdateHorarios(domainHorarios);
        }

        comercio.ClearDomainEvents();
        return comercio;
    }
}
