using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Domain.Usuarios;
using FluentValidation;

namespace AppGestionGastos.Application.Features.Usuarios.Obtener
{
    public sealed class ObtenerUsuariosQueryValidator : PagedQueryValidator<ObtenerUsuariosQuery>
    {
        public ObtenerUsuariosQueryValidator() =>
            RuleFor(x => x.Buscar).MaximumLength(Email.MaxLength);
    }
}
