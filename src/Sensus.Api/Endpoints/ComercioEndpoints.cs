using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Comercios.Commands.RegisterComercio;
using Sensus.Application.Comercios.Commands.UpdateMerchantProfile;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Comercios.Queries.GetMyComercio;
using Sensus.Application.Comercios.Queries.GetNearbyComercios;

namespace Sensus.Api.Endpoints;

public static class ComercioEndpoints
{
    public static IEndpointRouteBuilder MapComercioEndpoints(this IEndpointRouteBuilder app)
    {
        // Grupo REST estándar: /api/v1/comercios
        var group = app.MapGroup("/api/v1/comercios")
            .WithTags("Comercios (SE-003 / SE-004 / SE-007 / SE-008)");

        // SE-003 & SE-004: Registro de local y activación de rol comercial
        group.MapPost("/", async (
            [FromBody] RegisterComercioCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToHttpResult();
            }

            return Results.Created($"/api/v1/comercios/{result.Value.ComercioId}", result.Value);
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithName("RegisterComercio")
        .WithSummary("SE-003 & SE-004: Registra el local comercial (sin burocracia) y activa dinámicamente el rol Comerciante")
        .Produces<RegisterComercioResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status409Conflict);

        // SE-004: Consulta de datos de mi comercio para conmutador de vista
        group.MapGet("/me", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetMyComercioQuery();
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.MerchantOnlyPolicy)
        .WithName("GetMyComercio")
        .WithSummary("SE-004: Obtiene los datos del comercio propio del usuario autenticado")
        .Produces<ComercioDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);

        // SE-007: Actualización de mi comercio (alias bajo /comercios/me)
        group.MapPut("/me", async (
            [FromBody] UpdateMerchantProfileCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.MerchantOnlyPolicy)
        .WithName("UpdateMyComercio")
        .WithSummary("SE-007: Edita datos del local y recalcula coordenadas espaciales")
        .Produces<ComercioDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        // SE-008: Búsqueda de comercios cercanos por radio (máximo 2 km para invitados)
        group.MapGet("/nearby", async (
            [FromQuery] double lat,
            [FromQuery] double lng,
            [FromQuery] int? radioMetros,
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetNearbyComerciosQuery(lat, lng, radioMetros);
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.GuestOrRegisteredPolicy)
        .WithName("GetNearbyComercios")
        .WithSummary("SE-008: Consulta comercios por proximidad espacial (restringido a 2 km para invitados)")
        .Produces<IReadOnlyList<ComercioDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

        // Alias de compatibilidad con la propuesta de endpoints específicos (/api/v1/comercio/registro y /api/v1/comercio/mi-comercio)
        var legacyGroup = app.MapGroup("/api/v1/comercio")
            .WithTags("Comercios (SE-003 / SE-004 - Rutas Alternativas)")
            .RequireAuthorization();

        legacyGroup.MapPost("/registro", async (
            [FromBody] RegisterComercioCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToHttpResult();
            }

            return Results.Created($"/api/v1/comercios/{result.Value.ComercioId}", result.Value);
        })
        .RequireAuthorization(SecurityExtensions.RegisteredUserPolicy)
        .WithName("RegisterComercioLegacy")
        .WithSummary("Alias para registro de local comercial (SE-003 & SE-004)")
        .Produces<RegisterComercioResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status409Conflict);

        legacyGroup.MapGet("/mi-comercio", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetMyComercioQuery();
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .RequireAuthorization(SecurityExtensions.MerchantOnlyPolicy)
        .WithName("GetMyComercioLegacy")
        .WithSummary("Alias para consulta del comercio propio del usuario (SE-004)")
        .Produces<ComercioDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}
