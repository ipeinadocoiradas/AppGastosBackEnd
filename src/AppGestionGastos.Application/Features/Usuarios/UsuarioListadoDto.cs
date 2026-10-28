

namespace AppGestionGastos.Application.Features.Usuarios
{
    public sealed record UsuarioListadoDto(Guid Id, string Nombre, string Email, IReadOnlyCollection<string> Roles);
}
