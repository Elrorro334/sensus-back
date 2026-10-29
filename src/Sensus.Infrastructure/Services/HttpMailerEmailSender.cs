using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Infrastructure.Services;

public sealed class HttpMailerEmailSender : IEmailSender
{
    private readonly HttpClient _httpClient;
    private readonly MailerSettings _settings;
    private readonly ILogger<HttpMailerEmailSender> _logger;

    public HttpMailerEmailSender(
        HttpClient httpClient,
        IOptions<MailerSettings> options,
        ILogger<HttpMailerEmailSender> logger)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? textBody = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
        {
            _logger.LogInformation(
                "[MODO DESARROLLO / MAILER SIMULADO] Se solicitó envío de correo. Asunto: {Subject}. Destinatario redactado por seguridad. (Configure 'MailerSettings:BaseUrl' para envíos reales vía Api.Mailer.Invitation).",
                subject);
            return;
        }

        var requestUri = new Uri(new Uri(_settings.BaseUrl.TrimEnd('/') + "/"), "api/mail/sensus/send");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            request.Headers.Add("X-Api-Key", _settings.ApiKey);
        }

        request.Content = JsonContent.Create(new
        {
            To = toEmail,
            Subject = subject,
            Html = htmlBody,
            Text = textBody
        });

        // Intentos con resiliencia básica
        const int maxRetries = 2;
        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(_settings.TimeoutSeconds > 0 ? _settings.TimeoutSeconds : 5));

                var response = await _httpClient.SendAsync(request, cts.Token);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Correo enviado exitosamente vía Api.Mailer.Invitation.");
                    return;
                }

                if ((int)response.StatusCode >= 500 && attempt < maxRetries)
                {
                    _logger.LogWarning("Fallo transitorio (código {StatusCode}) al contactar Api.Mailer.Invitation. Reintentando intento {Attempt}/{MaxRetries}...", response.StatusCode, attempt, maxRetries);
                    await Task.Delay(500 * attempt, cancellationToken);
                    continue;
                }

                _logger.LogError("Error al enviar correo vía Api.Mailer.Invitation. Código HTTP: {StatusCode}", response.StatusCode);
                throw new HttpRequestException($"Error al enviar correo vía Api.Mailer.Invitation: {response.StatusCode}");
            }
            catch (Exception ex) when (attempt < maxRetries && ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "Excepción en intento {Attempt} al enviar correo vía Api.Mailer.Invitation. Reintentando...", attempt);
                await Task.Delay(500 * attempt, cancellationToken);
            }
        }
    }
}
