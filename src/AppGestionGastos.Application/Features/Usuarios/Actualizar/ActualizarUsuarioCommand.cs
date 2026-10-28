

namespace AppGestionGastos.Application.Features.Usuarios.Actualizar
{
    public sealed record ActualizarUsuarioCommand(Guid Id, string Nombre, string Email, string[] Roles) : ICommand<Result<UsuarioListadoDto>>;
}
