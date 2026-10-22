using System.Security.Claims;
using Sensus.Application.Common.Interfaces;

namespace Sensus.Api.Services;

/// <summary>
/// Extrae la identidad del usuario actual a partir del HttpContext.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
