using Sensus.Domain.Common;

namespace Sensus.Domain.ValueObjects;

/// <summary>
/// Value Object que modela el horario de atención para un día de la semana (SE-007).
/// </summary>
public sealed class HorarioDia : ValueObject
{
    public DayOfWeek Dia { get; private set; }
    public TimeOnly? HoraApertura { get; private set; }
    public TimeOnly? HoraCierre { get; private set; }
    public bool EstaCerrado { get; private set; }

    private HorarioDia()
    {
    }

    private HorarioDia(
        DayOfWeek dia,
        TimeOnly? horaApertura,
        TimeOnly? horaCierre,
        bool estaCerrado)
    {
        Dia = dia;
        HoraApertura = horaApertura;
        HoraCierre = horaCierre;
        EstaCerrado = estaCerrado;
    }

    public static Result<HorarioDia> Abierto(DayOfWeek dia, TimeOnly apertura, TimeOnly cierre)
    {
        if (apertura >= cierre)
        {
            return Result.Failure<HorarioDia>(
                Error.Validation("Horario.Invalido", "La hora de apertura debe ser anterior a la hora de cierre."));
        }

        return Result.Success(new HorarioDia(dia, apertura, cierre, estaCerrado: false));
    }

    public static Result<HorarioDia> Cerrado(DayOfWeek dia)
    {
        return Result.Success(new HorarioDia(dia, null, null, estaCerrado: true));
    }

    public bool EsAbiertoEn(DayOfWeek dia, TimeOnly hora)
    {
        if (EstaCerrado || Dia != dia || !HoraApertura.HasValue || !HoraCierre.HasValue)
        {
            return false;
        }

        return hora >= HoraApertura.Value && hora < HoraCierre.Value;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Dia;
        yield return HoraApertura;
        yield return HoraCierre;
        yield return EstaCerrado;
    }
}
