using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Usuarios;


namespace AppGestionGastos.Application.Features.Usuarios.Actualizar
{
    internal sealed class ActualizarUsuarioCommandHandler(
     IUsuarioRepository usuarios, ICurrentUser currentUser, IUnitOfWork unitOfWork)
     : ICommandHandler<ActualizarUsuarioCommand, Result<UsuarioListadoDto>>
    {
        public async Task<Result<UsuarioListadoDto>> HandleAsync(
            ActualizarUsuarioCommand command, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } actualId) return UsuarioErrors.NoAutenticado;

            var usuario = await usuarios.GetByIdAsync(command.Id, cancellationToken);
            if (usuario is null) return UsuarioErrors.NoEncontrado;

            // Un administrador no puede dejarse a sí mismo sin acceso de administración.
            if (usuario.Id == actualId && !command.Roles.Contains(Rol.Admin))
            {
                return UsuarioErrors.NoPuedeQuitarseAdmin;
            }

            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure) return emailResult.Error;

            var conEseEmail = await usuarios.GetByEmailAsync(emailResult.Value, cancellationToken);
            if (conEseEmail is not null && conEseEmail.Id != usuario.Id)
            {
                return UsuarioErrors.EmailYaRegistrado;
            }

            var resultado = usuario.Actualizar(command.Nombre, emailResult.Value, command.Roles);
            if (resultado.IsFailure) return resultado.Error;

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new UsuarioListadoDto(usuario.Id, usuario.Nombre, usuario.Email.Value, usuario.Roles);
        }
    }
}
