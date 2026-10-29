using FluentValidation;

namespace Sensus.Application.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre completo debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre completo no puede exceder 100 caracteres.")
            .Must(nombre => !nombre.Any(char.IsControl)).WithMessage("El nombre completo no puede contener caracteres de control.");

        When(x => !string.IsNullOrWhiteSpace(x.FotoPerfilUrl), () =>
        {
            RuleFor(x => x.FotoPerfilUrl!)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                             && uri.Scheme == Uri.UriSchemeHttps
                             && uri.Host.EndsWith("cloudinary.com", StringComparison.OrdinalIgnoreCase))
                .WithMessage("La foto de perfil debe ser una URL segura HTTPS alojada en Cloudinary.");
        });
    }
}
