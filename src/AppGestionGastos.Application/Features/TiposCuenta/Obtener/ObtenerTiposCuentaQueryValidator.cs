using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;


namespace AppGestionGastos.Application.Features.TiposCuenta.Obtener
{
    public sealed class ObtenerTiposCuentaQueryValidator : PagedQueryValidator<ObtenerTiposCuentaQuery>
    {
        public ObtenerTiposCuentaQueryValidator() => RuleFor(x => x.Buscar).MaximumLength(50);
    }
}
