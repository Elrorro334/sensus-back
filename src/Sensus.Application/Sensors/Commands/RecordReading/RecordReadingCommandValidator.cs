using FluentValidation;

namespace Sensus.Application.Sensors.Commands.RecordReading;

public sealed class RecordReadingCommandValidator : AbstractValidator<RecordReadingCommand>
{
    public RecordReadingCommandValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty().WithMessage("El ID del sensor es obligatorio.");

        RuleFor(x => x.Reading)
            .NotNull().WithMessage("El valor de la lectura es obligatorio.");
    }
}
