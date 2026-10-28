using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Usuarios
{
    public interface IUsuarioReadRepository
    {
        Task<PagedResult<UsuarioListadoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
