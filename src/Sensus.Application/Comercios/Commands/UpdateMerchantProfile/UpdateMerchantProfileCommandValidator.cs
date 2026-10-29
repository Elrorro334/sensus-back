using FluentValidation;

namespace Sensus.Application.Comercios.Commands.UpdateMerchantProfile;

public sealed class UpdateMerchantProfileCommandValidator : AbstractValidator<UpdateMerchantProfileCommand>
{
    public UpdateMerchantProfileCommandValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.NombreComercial), () =>
        {
            RuleFor(x => x.NombreComercial!)
                .MinimumLength(2).WithMessage("El nombre comercial debe tener al menos 2 caracteres.")
                .MaximumLength(100).WithMessage("El nombre comercial no puede exceder 100 caracteres.");
        });

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección del local es obligatoria.")
            .MinimumLength(5).WithMessage("La dirección debe tener al menos 5 caracteres.")
            .MaximumLength(250).WithMessage("La dirección no puede exceder 250 caracteres.");

        RuleFor(x => x.CodigoPostal)
            .NotEmpty().WithMessage("El código postal es obligatorio.")
            .Matches(@"^\d{5}$").WithMessage("El código postal debe contener exactamente 5 dígitos numéricos.");
    }
}
