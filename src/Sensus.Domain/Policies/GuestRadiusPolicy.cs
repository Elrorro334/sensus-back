namespace Sensus.Domain.Policies;

/// <summary>
/// Política de dominio para restricción estricta de radio en modo invitado (SE-008).
/// </summary>
public static class GuestRadiusPolicy
{
    public const int GuestMaxRadiusMeters = 2000;
    public const int RegisteredMaxRadiusMeters = 50000;
    public const int DefaultRadiusMeters = 2000;

    public static int ResolveRadius(bool isGuest, int requestedRadiusMeters)
    {
        if (requestedRadiusMeters <= 0)
        {
            return DefaultRadiusMeters;
        }

        if (isGuest)
        {
            return Math.Min(requestedRadiusMeters, GuestMaxRadiusMeters);
        }

        return Math.Min(requestedRadiusMeters, RegisteredMaxRadiusMeters);
    }
}
