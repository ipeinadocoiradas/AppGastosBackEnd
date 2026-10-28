using FluentValidation;

namespace AppGestionGastos.Application.Features.Bancos.Actualizar
{
    public sealed class ActualizarBancoCommandValidator : AbstractValidator<ActualizarBancoCommand>
    {
        public ActualizarBancoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CodigoInstitucional).NotEmpty().MaximumLength(20);
        }
    }
}
