using FluentValidation;

namespace AppGestionGastos.Application.Features.Cuentas.Actualizar
{
    public sealed class ActualizarCuentaCommandValidator : AbstractValidator<ActualizarCuentaCommand>
    {
        public ActualizarCuentaCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.BancoId).NotEmpty();
            RuleFor(x => x.TipoCuentaId).NotEmpty();
            RuleFor(x => x.NumeroCuenta).NotEmpty().MaximumLength(34);
        }
    }
}
