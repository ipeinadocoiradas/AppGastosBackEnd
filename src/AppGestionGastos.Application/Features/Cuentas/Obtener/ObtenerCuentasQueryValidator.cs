using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;


namespace AppGestionGastos.Application.Features.Cuentas.Obtener
{
    internal class ObtenerCuentasQueryValidator : PagedQueryValidator<ObtenerCuentasQuery>
    {
        public ObtenerCuentasQueryValidator()
        {
            RuleFor(x => x.Buscar).MaximumLength(100);            
        }
    }
}
