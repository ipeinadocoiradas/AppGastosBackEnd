using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Features.Usuarios.Eliminar
{
    internal sealed class EliminarUsuarioCommandHandler(
     IUsuarioRepository usuarios, ICurrentUser currentUser, IUnitOfWork unitOfWork)
     : ICommandHandler<EliminarUsuarioCommand, Result>
    {
        public async Task<Result> HandleAsync(
            EliminarUsuarioCommand command, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } actualId) return UsuarioErrors.NoAutenticado;
            if (command.Id == actualId) return UsuarioErrors.NoPuedeEliminarseASiMismo;

            var usuario = await usuarios.GetByIdAsync(command.Id, cancellationToken);
            if (usuario is null) return UsuarioErrors.NoEncontrado;

            if (await usuarios.EstaEnUsoAsync(usuario.Id, cancellationToken)) return UsuarioErrors.TieneCuentas;

            usuarios.Remove(usuario);

            return await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
