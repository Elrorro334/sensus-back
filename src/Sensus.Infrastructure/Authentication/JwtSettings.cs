namespace Sensus.Infrastructure.Authentication;

public sealed class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Secret { get; set; } = "SensusSuperSecretKey2026_Minimum32CharactersLong!";
    public string Issuer { get; set; } = "SensusApi";
    public string Audience { get; set; } = "SensusApp";
    public int ExpiryMinutes { get; set; } = 1440; // 24 horas por defecto
}
