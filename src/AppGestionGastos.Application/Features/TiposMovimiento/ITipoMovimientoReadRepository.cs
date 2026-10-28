using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.TiposMovimiento
{
    public interface ITipoMovimientoReadRepository
    {
        Task<PagedResult<TipoMovimientoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
