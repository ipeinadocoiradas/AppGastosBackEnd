using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.TiposCuenta
{
    public interface ITipoCuentaReadRepository
    {
        Task<PagedResult<TipoCuentaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
