namespace Sensus.Application.Common.Interfaces;

/// <summary>
/// Contrato para el envío de correos electrónicos transaccionales (recuperación de contraseña, notificaciones).
/// </summary>
public interface IEmailSender
{
    Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? textBody = null,
        CancellationToken cancellationToken = default);
}
