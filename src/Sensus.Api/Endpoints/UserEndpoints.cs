using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.DeleteAccount;
using Sensus.Application.Users.Commands.GeneratePhotoSignature;
using Sensus.Application.Users.Commands.UpdateProfile;
using Sensus.Application.Users.DTOs;
using Sensus.Application.Users.Queries.GetProfile;

namespace Sensus.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        MapUserGroup(app.MapGroup("/api/v1/users"), isV1: true);
        MapUserGroup(app.MapGroup("/api/users"), isV1: false); // Alias de compatibilidad

        return app;
    }

    private static void MapUserGroup(RouteGroupBuilder group, bool isV1)
    {
        group.WithTags("Usuarios y Perfil (SE-006 / SE-009)");

        // SE-006: Obtener información del perfil
        var getProfile = group.MapGet("/profile", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new GetProfileQuery(), ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithSummary("SE-006: Consulta el perfil del usuario autenticado")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);

        if (isV1) getProfile.WithName("GetProfile");

        // SE-006: Actualizar información personal
        var updateProfile = group.MapPut("/profile", async (
            [FromBody] UpdateProfileCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithSummary("SE-006: Actualiza el nombre completo y/o la foto de perfil en Cloudinary")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);

        if (isV1) updateProfile.WithName("UpdateProfile");

        // SE-006: Solicitar firma de subida para Cloudinary
        var photoSignature = group.MapPost("/photo-signature", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new GeneratePhotoSignatureCommand(), ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithSummary("SE-006: Genera parámetros firmados para subida directa de fotos a Cloudinary")
        .Produces<CloudinarySignatureResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);

        if (isV1) photoSignature.WithName("GeneratePhotoSignature");

        // SE-009: Eliminación definitiva de cuenta y datos asociados
        var deleteAccount = group.MapDelete("/account", async (
            [FromBody] DeleteAccountCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithSummary("SE-009: Elimina la cuenta y todos sus datos personales y comerciales de forma definitiva previa confirmación de contraseña")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);

        if (isV1) deleteAccount.WithName("DeleteAccount");
    }
}
