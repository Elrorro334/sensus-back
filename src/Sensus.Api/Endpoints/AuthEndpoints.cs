using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Users.Commands.Login;
using Sensus.Application.Users.Commands.RegisterUser;
using Sensus.Application.Users.DTOs;

namespace Sensus.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth")
            .WithTags("Autenticación (SE-001 / SE-002)");

        // SE-001: Registro de usuario
        group.MapPost("/register", async (
            [FromBody] RegisterUserCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToHttpResult();
            }

            return Results.Created($"/api/v1/users/{result.Value.User.Id}", result.Value);
        })
        .WithName("RegisterUser")
        .WithSummary("SE-001: Registra una nueva cuenta con correo y contraseña bajo el modelo unificado")
        .Produces<AuthResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);

        // SE-002: Inicio de sesión
        group.MapPost("/login", async (
            [FromBody] LoginCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .WithName("LoginUser")
        .WithSummary("SE-002: Inicia sesión de forma segura y genera token JWT")
        .Produces<AuthResponseDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden);

        // Verificación de sesión / Perfil actual autenticado
        group.MapGet("/me", (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                         user.FindFirstValue("sub");
            var email = user.FindFirstValue(ClaimTypes.Email) ??
                        user.FindFirstValue("email");
            var name = user.FindFirstValue(ClaimTypes.Name) ??
                       user.FindFirstValue("name");
            var roles = user.FindAll(ClaimTypes.Role)
                .Concat(user.FindAll("role"))
                .Select(c => c.Value)
                .Distinct()
                .ToList();

            return Results.Ok(new
            {
                UserId = userId,
                NombreCompleto = name,
                Correo = email,
                Roles = roles
            });
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithSummary("Obtiene los datos del usuario autenticado a partir del token JWT")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

        return app;
    }
}
