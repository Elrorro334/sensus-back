using System.Security.Claims;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Enums;

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

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
        User?.FindFirstValue("sub");

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email) ??
        User?.FindFirstValue("email");

    public IReadOnlyList<string> Roles =>
        User?.FindAll(ClaimTypes.Role)
            .Concat(User?.FindAll("role") ?? [])
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? [];

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public bool IsGuest =>
        Roles.Contains(RoleType.Invitado, StringComparer.OrdinalIgnoreCase);

    public bool IsMerchant =>
        Roles.Contains(RoleType.Comerciante, StringComparer.OrdinalIgnoreCase);
}
