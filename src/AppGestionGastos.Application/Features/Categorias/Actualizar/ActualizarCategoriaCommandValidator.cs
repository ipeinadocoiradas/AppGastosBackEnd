using FluentValidation;

namespace AppGestionGastos.Application.Features.Categorias.Actualizar
{
    public sealed class ActualizarCategoriaCommandValidator : AbstractValidator<ActualizarCategoriaCommand>
    {
        public ActualizarCategoriaCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Descripcion).MaximumLength(250);
        }
    }
}
