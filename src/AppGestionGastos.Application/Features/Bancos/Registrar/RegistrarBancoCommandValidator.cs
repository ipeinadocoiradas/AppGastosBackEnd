using FluentValidation;

namespace AppGestionGastos.Application.Features.Bancos.Crear;

public sealed class RegistrarBancoCommandValidator : AbstractValidator<RegistrarBancoCommand>
{
    public RegistrarBancoCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CodigoInstitucional).NotEmpty().MaximumLength(20);
    }
}