namespace Sensus.Domain.Enums;

/// <summary>
/// Roles del modelo unificado (Airbnb) para consumidores y comerciantes.
/// </summary>
public static class RoleType
{
    public const string Consumidor = "Consumidor";
    public const string Comerciante = "Comerciante";
    public const string Administrador = "Administrador";

    public static readonly IReadOnlyList<string> All = [Consumidor, Comerciante, Administrador];
}
