using FluentValidation;

namespace Sensus.Application.Sensors.Commands.CreateSensor;

public sealed class CreateSensorCommandValidator : AbstractValidator<CreateSensorCommand>
{
    public CreateSensorCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("El código del sensor es obligatorio.")
            .MaximumLength(50).WithMessage("El código no puede exceder 50 caracteres.")
            .Matches(@"^[a-zA-Z0-9_\-]+$").WithMessage("El código solo puede contener letras, números, guiones y guiones bajos.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del sensor es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de sensor especificado no es válido.");
    }
}
