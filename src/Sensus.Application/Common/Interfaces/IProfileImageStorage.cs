namespace Sensus.Application.Common.Interfaces;

public sealed record CloudinarySignatureResponse(
    string CloudName,
    string ApiKey,
    long Timestamp,
    string Folder,
    string Signature);

public interface IProfileImageStorage
{
    CloudinarySignatureResponse GenerateUploadSignature(Guid userId, long timestampSeconds);
    Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
}
