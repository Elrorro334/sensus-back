namespace Sensus.Domain.Enums;

/// <summary>
/// Roles del modelo unificado (Airbnb) para consumidores, comerciantes e invitados temporales.
/// </summary>
public static class RoleType
{
    public const string Consumidor = "Consumidor";
    public const string Comerciante = "Comerciante";
    public const string Administrador = "Administrador";
    public const string Invitado = "Invitado";

    public static readonly IReadOnlyList<string> All = [Consumidor, Comerciante, Administrador, Invitado];
}
