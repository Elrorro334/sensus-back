using FluentValidation;

namespace Sensus.Application.Users.Commands.DeleteAccount;

public sealed class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Debe proporcionar su contraseña actual para confirmar la eliminación de la cuenta.");
    }
}
