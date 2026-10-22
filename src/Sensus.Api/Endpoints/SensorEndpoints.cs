using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Common.Models;
using Sensus.Application.Sensors.Commands.CreateSensor;
using Sensus.Application.Sensors.Commands.RecordReading;
using Sensus.Application.Sensors.DTOs;
using Sensus.Application.Sensors.Queries.GetSensorById;
using Sensus.Application.Sensors.Queries.GetSensors;
using Sensus.Domain.Enums;

namespace Sensus.Api.Endpoints;

public static class SensorEndpoints
{
    public static IEndpointRouteBuilder MapSensorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/sensors")
            .WithTags("Sensors");

        // GET /api/v1/sensors
        group.MapGet("/", async (
            [FromQuery] string? searchTerm,
            [FromQuery] SensorStatus? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            ISender sender = default!,
            CancellationToken ct = default) =>
        {
            var query = new GetSensorsQuery(searchTerm, status, pageNumber, pageSize);
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .WithName("GetSensors")
        .WithSummary("Obtiene una lista paginada de sensores con filtros opcionales")
        .Produces<PagedResult<SensorDto>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        // GET /api/v1/sensors/{id}
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetSensorByIdQuery(id);
            var result = await sender.Send(query, ct);
            return result.ToHttpResult();
        })
        .WithName("GetSensorById")
        .WithSummary("Obtiene el detalle de un sensor por su ID único")
        .Produces<SensorDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        // POST /api/v1/sensors
        group.MapPost("/", async (
            [FromBody] CreateSensorCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToHttpResult();
            }

            return Results.Created($"/api/v1/sensors/{result.Value.Id}", result.Value);
        })
        .WithName("CreateSensor")
        .WithSummary("Crea y registra un nuevo sensor en el sistema")
        .Produces<SensorDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);

        // POST /api/v1/sensors/{id}/readings
        group.MapPost("/{id:guid}/readings", async (
            Guid id,
            [FromBody] RecordReadingRequest request,
            ISender sender,
            CancellationToken ct) =>
        {
            var command = new RecordReadingCommand(id, request.Reading, request.TimestampUtc);
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .WithName("RecordSensorReading")
        .WithSummary("Registra una nueva lectura de medición para el sensor")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);

        return app;
    }
}

public sealed record RecordReadingRequest(decimal Reading, DateTime? TimestampUtc = null);
