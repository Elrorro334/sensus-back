using FluentValidation;

namespace Sensus.Application.Sensors.Queries.GetSensors;

public sealed class GetSensorsQueryValidator : AbstractValidator<GetSensorsQuery>
{
    public GetSensorsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("El número de página debe ser mayor o igual a 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("El tamaño de página debe estar entre 1 y 100.");
    }
}
