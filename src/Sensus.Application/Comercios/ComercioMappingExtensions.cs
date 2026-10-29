using Sensus.Application.Comercios.DTOs;
using Sensus.Domain.Entities;

namespace Sensus.Application.Comercios;

public static class ComercioMappingExtensions
{
    public static ComercioDto ToDto(this Comercio comercio)
    {
        return new ComercioDto(
            comercio.Id,
            comercio.UsuarioId,
            comercio.NombreComercial,
            comercio.Direccion,
            comercio.CodigoPostal,
            comercio.Latitud,
            comercio.Longitud,
            comercio.Activo,
            comercio.FechaRegistro);
    }
}
