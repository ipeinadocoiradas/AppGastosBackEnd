using FluentValidation;


namespace AppGestionGastos.Application.Features.Cuentas.Registrar
{
    public sealed class RegistrarCuentaCommandValidator : AbstractValidator<RegistrarCuentaCommand>
    {
        public RegistrarCuentaCommandValidator() {
            RuleFor(x => x.NumeroCuenta).NotEmpty().MaximumLength(34);

            RuleFor(x => x.SaldoActual)
           .GreaterThan(0)
           .PrecisionScale(18, 2, ignoreTrailingZeros: true);

            RuleFor(x => x.BancoId).NotEmpty();
            RuleFor(x => x.TipoCuentaId).NotEmpty();

        }
    }
}
