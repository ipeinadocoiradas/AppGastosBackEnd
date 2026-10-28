using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.TiposCuenta.Obtener
{
    internal sealed class ObtenerTiposCuentaQueryHandler(ITipoCuentaReadRepository tiposCuenta)
    : IQueryHandler<ObtenerTiposCuentaQuery, Result<PagedResult<TipoCuentaDto>>>
    {
        public async Task<Result<PagedResult<TipoCuentaDto>>> HandleAsync(
            ObtenerTiposCuentaQuery query, CancellationToken cancellationToken = default) =>
            await tiposCuenta.ListarAsync(query.Buscar, query.ToPageRequest(), cancellationToken);
    }
}
