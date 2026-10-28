using AppGestionGastos.Application.Common.Pagination;
using FluentValidation;


namespace AppGestionGastos.Application.Features.Categorias.Obtener
{
    public sealed class ObtenerCategoriasQueryValidator : PagedQueryValidator<ObtenerCategoriasQuery>
    {
        public ObtenerCategoriasQueryValidator() => RuleFor(x => x.Buscar).MaximumLength(100);
    }
}
