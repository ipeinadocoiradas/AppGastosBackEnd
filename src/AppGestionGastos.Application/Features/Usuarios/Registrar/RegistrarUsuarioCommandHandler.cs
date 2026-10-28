using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Features.Usuarios.Registrar;

internal sealed class RegistrarUsuarioCommandHandler(
    IUsuarioRepository usuarios,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegistrarUsuarioCommand, Result<UsuarioDto>>
{
    public async Task<Result<UsuarioDto>> HandleAsync(
        RegistrarUsuarioCommand command,
        CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure) return emailResult.Error;

        var email = emailResult.Value;

        if (await usuarios.ExisteEmailAsync(email, cancellationToken))
        {
            return UsuarioErrors.EmailYaRegistrado;
        }

        var usuarioResult = Usuario.Crear(
            command.Nombre,
            email,
            passwordHasher.Hash(command.Password),
            [Rol.Usuario]);

        if (usuarioResult.IsFailure) return usuarioResult.Error;

        var usuario = usuarioResult.Value;
        usuarios.Add(usuario);

        // Despacha UsuarioCreadoEvent y persiste en una única transacción.
        var commit = await unitOfWork.CommitAsync(cancellationToken);
        if (commit.IsFailure) return commit.Error;

        return new UsuarioDto(usuario.Id, usuario.Nombre, usuario.Email.Value);
    }
}
