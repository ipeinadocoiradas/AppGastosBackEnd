using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;

namespace AppGestionGastos.Application.Features.Movimientos.Obtener
{
    public sealed class ObtenerMovimientosQueryValidator : PagedQueryValidator<ObtenerMovimientosQuery>
    {
        public ObtenerMovimientosQueryValidator()
        {
            RuleFor(x => x.Hasta)
                .GreaterThanOrEqualTo(x => x.Desde)
                .When(x => x.Desde.HasValue && x.Hasta.HasValue)
                .WithMessage("'Hasta' no puede ser anterior a 'Desde'.");
        }
    }
}
