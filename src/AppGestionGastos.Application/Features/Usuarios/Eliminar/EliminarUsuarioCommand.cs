

namespace AppGestionGastos.Application.Features.Usuarios.Eliminar
{
    public sealed record EliminarUsuarioCommand(Guid Id) : ICommand<Result>;
}
