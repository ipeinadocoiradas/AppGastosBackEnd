using FluentValidation;

namespace AppGestionGastos.Application.Features.Categorias.Crear;

public sealed class RegistrarCategoriaCommandValidator : AbstractValidator<RegistrarCategoriaCommand>
{
    public RegistrarCategoriaCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Descripcion).MaximumLength(250);
    }
}