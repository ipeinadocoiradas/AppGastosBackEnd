using FluentValidation;

namespace AppGestionGastos.Application.Features.TiposCuenta.Actualizar
{
    public sealed class ActualizarTipoCuentaCommandValidator : AbstractValidator<ActualizarTipoCuentaCommand>
    {
        public ActualizarTipoCuentaCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Descripcion).MaximumLength(250);
        }
    }
}
