using AppGestionGastos.Domain.Movimientos;
using FluentValidation;

namespace AppGestionGastos.Application.Features.Movimientos.Registrar;

public sealed class RegistrarMovimientoCommandValidator : AbstractValidator<RegistrarMovimientoCommand>
{
    public RegistrarMovimientoCommandValidator()
    {
        RuleFor(x => x.Monto)
            .GreaterThan(0)
            .PrecisionScale(18, 2, ignoreTrailingZeros: true);

        RuleFor(x => x.CuentaId).NotEmpty();
        RuleFor(x => x.TipoMovimientoId).NotEmpty();
        RuleFor(x => x.CategoriaId).NotEmpty();

        RuleFor(x => x.Descripcion)
            .MaximumLength(Movimiento.DescripcionMaxLength);

        RuleFor(x => x.FechaHora)
            .Must(fecha => fecha!.Value <= DateTimeOffset.UtcNow.AddMinutes(5))
            .When(x => x.FechaHora.HasValue)
            .WithMessage("La fecha del movimiento no puede estar en el futuro.");
    }
}
