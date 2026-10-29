using FluentValidation;

namespace Sensus.Application.Comercios.Commands.RegisterComercio;

/// <summary>
/// Validador estricto para SE-003: valida los datos del local sin solicitar trámites burocráticos.
/// </summary>
public sealed class RegisterComercioCommandValidator : AbstractValidator<RegisterComercioCommand>
{
    public RegisterComercioCommandValidator()
    {
        RuleFor(x => x.NombreComercial)
            .NotEmpty().WithMessage("El nombre comercial es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre comercial debe tener al menos 2 caracteres.")
            .MaximumLength(150).WithMessage("El nombre comercial no puede exceder 150 caracteres.");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección del comercio es obligatoria.")
            .MinimumLength(5).WithMessage("La dirección debe contener al menos 5 caracteres.")
            .MaximumLength(250).WithMessage("La dirección no puede exceder 250 caracteres.");

        RuleFor(x => x.CodigoPostal)
            .NotEmpty().WithMessage("El código postal es obligatorio.")
            .Matches(@"^[0-9a-zA-Z\s\-]{4,10}$").WithMessage("El código postal no tiene un formato válido (entre 4 y 10 caracteres alfanuméricos).");
    }
}
