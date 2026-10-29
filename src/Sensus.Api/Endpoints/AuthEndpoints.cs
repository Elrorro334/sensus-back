using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sensus.Api.Extensions;
using Sensus.Application.Users.Commands.CreateGuestSession;
using Sensus.Application.Users.Commands.Login;
using Sensus.Application.Users.Commands.RegisterUser;
using Sensus.Application.Users.Commands.RequestPasswordReset;
using Sensus.Application.Users.Commands.ResetPassword;
using Sensus.Application.Users.DTOs;

namespace Sensus.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        MapAuthGroup(app.MapGroup("/api/v1/auth"), isV1: true);
        MapAuthGroup(app.MapGroup("/api/auth"), isV1: false); // Alias sin prefijo v1 para compatibilidad

        return app;
    }

    private static void MapAuthGroup(RouteGroupBuilder group, bool isV1)
    {
        group.WithTags("Autenticación (SE-001 / SE-002 / SE-005 / SE-008)");

        // SE-001: Registro de usuario
        var register = group.MapPost("/register", async (
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
        .RequireRateLimiting(SecurityExtensions.AuthStrictPolicy)
        .WithSummary("SE-001: Registra una nueva cuenta con correo y contraseña bajo el modelo unificado")
        .Produces<AuthResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        if (isV1) register.WithName("RegisterUser");

        // SE-002: Inicio de sesión
        var login = group.MapPost("/login", async (
            [FromBody] LoginCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .RequireRateLimiting(SecurityExtensions.AuthStrictPolicy)
        .WithSummary("SE-002: Inicia sesión de forma segura y genera token JWT")
        .Produces<AuthResponseDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        if (isV1) login.WithName("LoginUser");

        // SE-008: Modo Invitado (omite registro y otorga token de 2 km)
        var guest = group.MapPost("/guest", async (
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(new CreateGuestSessionCommand(), ct);
            return result.ToHttpResult();
        })
        .RequireRateLimiting(SecurityExtensions.AuthStrictPolicy)
        .WithSummary("SE-008: Inicia una sesión de invitado con radio limitado a 2,000 metros")
        .Produces<GuestSessionResponseDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        if (isV1) guest.WithName("CreateGuestSession");

        // SE-005: Solicitud de recuperación de contraseña
        var recover = group.MapPost("/recover", async (
            [FromBody] RequestPasswordResetCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            if (result.IsFailure)
            {
                return result.ToHttpResult();
            }

            return Results.Accepted(value: new
            {
                message = "Si el correo electrónico está registrado, se enviará un enlace de recuperación con vigencia de 15 minutos."
            });
        })
        .RequireRateLimiting(SecurityExtensions.RecoverPolicy)
        .WithSummary("SE-005: Solicita el restablecimiento de contraseña enviando un enlace temporal de 15 minutos")
        .Produces(StatusCodes.Status202Accepted)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        if (isV1) recover.WithName("RequestPasswordReset");

        // SE-005: Restablecimiento de contraseña
        var reset = group.MapPost("/reset-password", async (
            [FromBody] ResetPasswordCommand command,
            ISender sender,
            CancellationToken ct) =>
        {
            var result = await sender.Send(command, ct);
            return result.ToHttpResult();
        })
        .RequireRateLimiting(SecurityExtensions.RecoverPolicy)
        .WithSummary("SE-005: Restablece la contraseña utilizando el token temporal de un solo uso")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        if (isV1) reset.WithName("ResetPassword");

        // Verificación de sesión / Perfil actual autenticado
        var me = group.MapGet("/me", (ClaimsPrincipal user) =>
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
        .WithSummary("Obtiene los datos del usuario autenticado a partir del token JWT")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

        if (isV1) me.WithName("GetCurrentUser");
    }
}
