using Sensus.Application.Common.Interfaces;

namespace Sensus.TestKit.Fakes;

public sealed class FakeEmailSender : IEmailSender
{
    public List<SentEmailRecord> SentEmails { get; } = [];

    public Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody,
        string? textBody = null,
        CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new SentEmailRecord(toEmail, subject, htmlBody, textBody));
        return Task.CompletedTask;
    }

    public void Clear() => SentEmails.Clear();

    public sealed record SentEmailRecord(string To, string Subject, string Html, string? Text);
}
