using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Comercios.Commands.RegisterComercio;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Comercios.Queries.GetMyComercio;

namespace Sensus.Api.Endpoints;

public static class ComercioEndpoints
{
    public static IEndpointRouteBuilder MapComercioEndpoints(this IEndpointRouteBuilder app)
    {
        // Grupo REST estándar: /api/v1/comercios
        var group = app.MapGroup("/api/v1/comercios")
            .WithTags("Comercios (SE-003 / SE-004)")
            .RequireAuthorization();

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
        .WithName("RegisterComercio")
        .WithSummary("SE-003 & SE-004: Registra el local comercial (sin burocracia) y activa dinámicamente el rol Comerciante")
        .Produces<RegisterComercioResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
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
        .WithName("GetMyComercio")
        .WithSummary("SE-004: Obtiene los datos del comercio propio del usuario autenticado")
        .Produces<ComercioDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);

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
        .WithName("RegisterComercioLegacy")
        .WithSummary("Alias para registro de local comercial (SE-003 & SE-004)")
        .Produces<RegisterComercioResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status409Conflict);

        legacyGroup.MapGet("/mi-comercio", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetMyComercioQuery();
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .WithName("GetMyComercioLegacy")
        .WithSummary("Alias para consulta del comercio propio del usuario (SE-004)")
        .Produces<ComercioDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }
}
