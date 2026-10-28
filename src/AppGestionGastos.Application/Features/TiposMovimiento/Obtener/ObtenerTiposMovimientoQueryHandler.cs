using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.TiposMovimiento.Obtener
{
    internal sealed class ObtenerTiposMovimientoQueryHandler(ITipoMovimientoReadRepository tiposMovimiento)
    : IQueryHandler<ObtenerTiposMovimientoQuery, Result<PagedResult<TipoMovimientoDto>>>
    {
        public async Task<Result<PagedResult<TipoMovimientoDto>>> HandleAsync(
            ObtenerTiposMovimientoQuery query, CancellationToken cancellationToken = default) =>
            await tiposMovimiento.ListarAsync(query.Buscar, query.ToPageRequest(), cancellationToken);
    }
}
