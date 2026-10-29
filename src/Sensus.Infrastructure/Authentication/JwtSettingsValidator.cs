using Microsoft.Extensions.Options;

namespace Sensus.Infrastructure.Authentication;

public sealed class JwtSettingsValidator : IValidateOptions<JwtSettings>
{
    private static readonly HashSet<string> KnownLeakedSecrets = new(StringComparer.Ordinal)
    {
        "SensusSuperSecretKey2026_Minimum32CharactersLong!",
        "SensusSuperSecretKey2026_Minimum32CharactersLong_Enterprise!"
    };

    public ValidateOptionsResult Validate(string? name, JwtSettings options)
    {
        if (string.IsNullOrWhiteSpace(options.Secret))
        {
            return ValidateOptionsResult.Fail("JwtSettings:Secret es obligatorio y no puede estar vacío.");
        }

        if (options.Secret.Length < 32)
        {
            return ValidateOptionsResult.Fail("JwtSettings:Secret debe tener al menos 32 caracteres.");
        }

        if (KnownLeakedSecrets.Contains(options.Secret))
        {
            return ValidateOptionsResult.Fail("JwtSettings:Secret no puede ser un valor histórico o filtrado. Debe rotar la clave secreta.");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail("JwtSettings:Issuer es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail("JwtSettings:Audience es obligatorio.");
        }

        if (options.ExpiryMinutes <= 0)
        {
            return ValidateOptionsResult.Fail("JwtSettings:ExpiryMinutes debe ser mayor a 0.");
        }

        return ValidateOptionsResult.Success;
    }
}
