using FluentAssertions;
using Sensus.Domain.ValueObjects;

namespace Sensus.Application.UnitTests.Comercios.Entities;

public class HorarioDiaTests
{
    [Fact]
    public void Abierto_ShouldSucceed_WhenAperturaIsBeforeCierre()
    {
        var apertura = new TimeOnly(9, 0);
        var cierre = new TimeOnly(18, 0);

        var result = HorarioDia.Abierto(DayOfWeek.Monday, apertura, cierre);

        result.IsSuccess.Should().BeTrue();
        result.Value.Dia.Should().Be(DayOfWeek.Monday);
        result.Value.HoraApertura.Should().Be(apertura);
        result.Value.HoraCierre.Should().Be(cierre);
        result.Value.EstaCerrado.Should().BeFalse();
    }

    [Fact]
    public void Abierto_ShouldFail_WhenAperturaIsAfterOrEqualToCierre()
    {
        var apertura = new TimeOnly(18, 0);
        var cierre = new TimeOnly(9, 0);

        var result = HorarioDia.Abierto(DayOfWeek.Monday, apertura, cierre);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Horario.Invalido");
    }

    [Fact]
    public void Cerrado_ShouldSucceed_WithNullHoras()
    {
        var result = HorarioDia.Cerrado(DayOfWeek.Sunday);

        result.IsSuccess.Should().BeTrue();
        result.Value.Dia.Should().Be(DayOfWeek.Sunday);
        result.Value.EstaCerrado.Should().BeTrue();
        result.Value.HoraApertura.Should().BeNull();
        result.Value.HoraCierre.Should().BeNull();
    }

    [Fact]
    public void EsAbiertoEn_ShouldReturnTrue_WhenWithinWorkingHours()
    {
        var horario = HorarioDia.Abierto(DayOfWeek.Wednesday, new TimeOnly(10, 0), new TimeOnly(20, 0)).Value;

        horario.EsAbiertoEn(DayOfWeek.Wednesday, new TimeOnly(14, 30)).Should().BeTrue();
        horario.EsAbiertoEn(DayOfWeek.Wednesday, new TimeOnly(9, 59)).Should().BeFalse();
        horario.EsAbiertoEn(DayOfWeek.Wednesday, new TimeOnly(20, 1)).Should().BeFalse();
        horario.EsAbiertoEn(DayOfWeek.Thursday, new TimeOnly(14, 30)).Should().BeFalse();
    }
}
