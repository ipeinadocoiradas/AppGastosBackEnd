using FluentValidation;

namespace AppGestionGastos.Application.Features.TiposCuenta.Crear;

public sealed class CrearTipoCuentaCommandValidator : AbstractValidator<RegistrarTipoCuentaCommand>
{
    public CrearTipoCuentaCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Descripcion).MaximumLength(250);
    }
}