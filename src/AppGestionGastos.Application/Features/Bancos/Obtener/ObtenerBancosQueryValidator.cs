using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;

namespace AppGestionGastos.Application.Features.Bancos.Obtener
{
    public sealed class ObtenerBancosQueryValidator : PagedQueryValidator<ObtenerBancosQuery>
    {
        public ObtenerBancosQueryValidator() => RuleFor(x => x.Buscar).MaximumLength(100);
    }
}
