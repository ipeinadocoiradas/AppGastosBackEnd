using AppGestionGastos.Domain.Usuarios;
using FluentValidation;

namespace AppGestionGastos.Application.Features.Usuarios.Actualizar
{
    public sealed class ActualizarUsuarioCommandValidator : AbstractValidator<ActualizarUsuarioCommand>
    {
        public ActualizarUsuarioCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(Email.MaxLength);
            RuleFor(x => x.Roles).NotEmpty();
            RuleForEach(x => x.Roles)
                .Must(Rol.EsValido)
                .WithMessage("El rol '{PropertyValue}' no es válido.");
        }
    }
}
