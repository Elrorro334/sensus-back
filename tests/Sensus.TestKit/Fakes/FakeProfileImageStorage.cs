using Sensus.Application.Common.Interfaces;

namespace Sensus.TestKit.Fakes;

public class FakeProfileImageStorage : IProfileImageStorage
{
    public List<string> DeletedPublicIds { get; } = [];

    public CloudinarySignatureResponse GenerateUploadSignature(Guid userId, long timestampSeconds)
    {
        return new CloudinarySignatureResponse(
            "fake-cloud",
            "fake-api-key",
            timestampSeconds,
            $"sensus/profiles/{userId}",
            "fake-signature-hash");
    }

    public Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
    {
        DeletedPublicIds.Add(publicId);
        return Task.FromResult(true);
    }
}
