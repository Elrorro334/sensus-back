namespace Sensus.Infrastructure.Services;

public sealed class MailerSettings
{
    public const string SectionName = "MailerSettings";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 5;
}
