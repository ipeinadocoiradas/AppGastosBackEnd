using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Usuarios.Obtener
{
    /// <param name="Buscar">Texto opcional: nombre (parcial) o email completo.</param>
    public sealed record ObtenerUsuariosQuery(
        int Page = 1,
        int PageSize = PageRequest.DefaultPageSize,
        string? Buscar = null) : IPagedQuery<UsuarioListadoDto>;
}
