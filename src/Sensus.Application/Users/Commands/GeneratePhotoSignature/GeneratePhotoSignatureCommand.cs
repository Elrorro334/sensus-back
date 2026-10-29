using MediatR;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;

namespace Sensus.Application.Users.Commands.GeneratePhotoSignature;

/// <summary>
/// SE-006: Genera una firma criptográfica para subida segura de foto de perfil directamente desde el cliente móvil a Cloudinary.
/// </summary>
public sealed record GeneratePhotoSignatureCommand : IRequest<Result<CloudinarySignatureResponse>>;

public sealed class GeneratePhotoSignatureCommandHandler : IRequestHandler<GeneratePhotoSignatureCommand, Result<CloudinarySignatureResponse>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IProfileImageStorage _imageStorage;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GeneratePhotoSignatureCommandHandler(
        ICurrentUserService currentUserService,
        IProfileImageStorage imageStorage,
        IDateTimeProvider dateTimeProvider)
    {
        _currentUserService = currentUserService;
        _imageStorage = imageStorage;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task<Result<CloudinarySignatureResponse>> Handle(GeneratePhotoSignatureCommand request, CancellationToken cancellationToken)
    {
        var rawUserId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(rawUserId) || !Guid.TryParse(rawUserId, out var userId))
        {
            return Task.FromResult(Result.Failure<CloudinarySignatureResponse>(
                Error.Unauthorized("Auth.Unauthorized", "Se requiere sesión activa para solicitar una firma de subida.")));
        }

        var timestamp = ((DateTimeOffset)_dateTimeProvider.UtcNow).ToUnixTimeSeconds();
        var signatureResponse = _imageStorage.GenerateUploadSignature(userId, timestamp);

        return Task.FromResult(Result.Success(signatureResponse));
    }
}
