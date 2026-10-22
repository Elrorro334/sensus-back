using Sensus.Application.Users.DTOs;
using Sensus.Domain.Entities;

namespace Sensus.Application.Users;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.NombreCompleto,
            user.Correo,
            user.Roles.ToList(),
            user.Estatus,
            user.FechaRegistro,
            user.UltimoAcceso);
    }
}
