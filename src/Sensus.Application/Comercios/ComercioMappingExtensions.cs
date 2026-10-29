using System.Globalization;
using Sensus.Application.Comercios.DTOs;
using Sensus.Domain.Entities;

namespace Sensus.Application.Comercios;

public static class ComercioMappingExtensions
{
    public static ComercioDto ToDto(this Comercio comercio)
    {
        var horariosDto = comercio.Horarios.Select(h => new HorarioDiaDto(
            h.Dia,
            h.HoraApertura?.ToString("HH:mm", CultureInfo.InvariantCulture),
            h.HoraCierre?.ToString("HH:mm", CultureInfo.InvariantCulture),
            h.EstaCerrado)).ToList();

        return new ComercioDto(
            comercio.Id,
            comercio.UsuarioId,
            comercio.NombreComercial,
            comercio.Direccion,
            comercio.CodigoPostal,
            comercio.Latitud,
            comercio.Longitud,
            comercio.Activo,
            comercio.FechaRegistro,
            horariosDto);
    }
}
