using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;


namespace AppGestionGastos.Application.Features.TiposMovimiento.Obtener
{
    public sealed class ObtenerTiposMovimientoQueryValidator
      : PagedQueryValidator<ObtenerTiposMovimientoQuery>
    {
        public ObtenerTiposMovimientoQueryValidator() =>
            RuleFor(x => x.Buscar).MaximumLength(50);
    }
}
