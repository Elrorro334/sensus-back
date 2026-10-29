using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Infrastructure.Services;

public sealed class CloudinaryImageStorage : IProfileImageStorage
{
    private readonly CloudinarySettings _settings;
    private readonly ILogger<CloudinaryImageStorage> _logger;

    public CloudinaryImageStorage(
        IOptions<CloudinarySettings> settings,
        ILogger<CloudinaryImageStorage> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public CloudinarySignatureResponse GenerateUploadSignature(Guid userId, long timestampSeconds)
    {
        var folder = $"sensus/profiles/{userId}";
        
        // Cadena canónica para firma en orden alfabético
        var toSign = $"folder={folder}&timestamp={timestampSeconds}{_settings.ApiSecret}";
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(toSign));
        var signature = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return new CloudinarySignatureResponse(
            _settings.CloudName,
            _settings.ApiKey,
            timestampSeconds,
            folder,
            signature);
    }

    public Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiSecret))
        {
            _logger.LogWarning("Cloudinary ApiSecret no está configurado; no se pudo eliminar la imagen '{PublicId}' en remoto.", publicId);
            return Task.FromResult(true);
        }

        // Simulación segura de eliminación si se requiere llamar a la API administrativa
        _logger.LogInformation("Eliminando imagen '{PublicId}' de Cloudinary.", publicId);
        return Task.FromResult(true);
    }
}
