using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Infrastructure.Services;

public sealed class CloudinaryImageStorage : IProfileImageStorage
{
    private static readonly HttpClient SharedHttpClient = new() { Timeout = TimeSpan.FromSeconds(15) };
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

    public async Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(publicId))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(_settings.ApiSecret) ||
            string.IsNullOrWhiteSpace(_settings.ApiKey) ||
            string.IsNullOrWhiteSpace(_settings.CloudName))
        {
            _logger.LogWarning("Credenciales de Cloudinary no están completas; se omite eliminación remota de '{PublicId}'.", publicId);
            return true;
        }

        try
        {
            var resolvedPublicId = ExtractPublicId(publicId);
            var timestampSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var toSign = $"public_id={resolvedPublicId}&timestamp={timestampSeconds}{_settings.ApiSecret}";
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(toSign));
            var signature = Convert.ToHexString(hashBytes).ToLowerInvariant();

            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["public_id"] = resolvedPublicId,
                ["api_key"] = _settings.ApiKey,
                ["timestamp"] = timestampSeconds.ToString(CultureInfo.InvariantCulture),
                ["signature"] = signature
            });

            var destroyUrl = $"https://api.cloudinary.com/v1_1/{_settings.CloudName}/image/destroy";
            using var response = await SharedHttpClient.PostAsync(destroyUrl, content, cancellationToken);
            _logger.LogInformation("Imagen '{PublicId}' eliminada de Cloudinary (HTTP {StatusCode}).", resolvedPublicId, (int)response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error al intentar eliminar la imagen '{PublicId}' de Cloudinary.", publicId);
            return false;
        }
    }

    private static string ExtractPublicId(string urlOrPublicId)
    {
        var uploadMarker = "/upload/";
        var idx = urlOrPublicId.IndexOf(uploadMarker, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return urlOrPublicId;
        }

        var path = urlOrPublicId[(idx + uploadMarker.Length)..];
        path = Regex.Replace(path, @"^v\d+/", string.Empty);
        var lastDot = path.LastIndexOf('.');
        if (lastDot > 0)
        {
            path = path[..lastDot];
        }

        return path;
    }
}
