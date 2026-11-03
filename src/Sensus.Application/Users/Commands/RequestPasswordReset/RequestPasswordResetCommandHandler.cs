using System.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Logging;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.Users.Commands.RequestPasswordReset;

public sealed class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IEmailSender _emailSender;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<RequestPasswordResetCommandHandler> _logger;

    public RequestPasswordResetCommandHandler(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IEmailSender emailSender,
        IDateTimeProvider dateTimeProvider,
        ILogger<RequestPasswordResetCommandHandler> logger)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _emailSender = emailSender;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Correo.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        // Anti-enumeración de usuarios: Si el usuario no existe o está inactivo, retornamos éxito genérico
        if (user is null || !user.Estatus)
        {
            _logger.LogInformation("Solicitud de recuperación para correo no registrado o inactivo. Respuesta genérica emitida.");
            return Result.Success();
        }

        // Invalidar tokens anteriores pendientes
        await _tokenRepository.InvalidateExistingTokensForUserAsync(user.Id, cancellationToken);

        // Generar token seguro CSPRNG de 32 bytes (64 caracteres hexadecimales)
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var rawToken = Convert.ToHexString(tokenBytes).ToLowerInvariant();

        var nowUtc = _dateTimeProvider.UtcNow;
        var tokenResult = PasswordResetToken.Create(user.Id, user.Correo, rawToken, nowUtc, expiryMinutes: 15);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }

        await _tokenRepository.AddAsync(tokenResult.Value, cancellationToken);

        // Construir plantilla y disparar correo
        var resetLink = $"https://rodnix.com.mx/sensus/reset?token={rawToken}";
        var appSchemeLink = $"sensus://reset?token={rawToken}";
        var htmlBody = $@"
<div style='font-family: sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #EFE9FA; border-radius: 16px;'>
    <h2 style='color: #4A148C;'>Recuperación de contraseña de Sensus</h2>
    <p>Hola <strong>{user.NombreCompleto}</strong>,</p>
    <p>Recibimos una solicitud para restablecer tu contraseña. Haz clic en el botón siguiente para abrir la app y definir una nueva contraseña:</p>
    <p style='text-align: center; margin: 30px 0;'>
        <a href='{resetLink}' style='background-color: #4A148C; color: white; padding: 14px 28px; text-decoration: none; border-radius: 12px; font-weight: bold; display: inline-block;'>Abrir en la App Sensus</a>
    </p>
    <p style='text-align: center; margin: 10px 0;'>
        <a href='{appSchemeLink}' style='color: #4A148C; font-size: 13px; text-decoration: underline;'>¿No abre automáticamente? Toca aquí (sensus://)</a>
    </p>
    <p style='color: #716B7C; font-size: 14px;'>Este enlace es de un solo uso y vencerá en <strong>15 minutos</strong>.</p>
    <p style='color: #9A94A8; font-size: 12px;'>Si no solicitaste este cambio, puedes ignorar este mensaje de forma segura.</p>
</div>";

        await _emailSender.SendEmailAsync(
            user.Correo,
            "Restablece tu contraseña - Sensus",
            htmlBody,
            $"Restablece tu contraseña en la App Sensus: {resetLink} o {appSchemeLink} (vigencia de 15 minutos).",
            cancellationToken);

        return Result.Success();
    }
}
