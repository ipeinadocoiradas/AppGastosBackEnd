using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.Bancos.Obtener
{
    internal sealed class ObtenerBancosQueryHandler(IBancoReadRepository bancos)
        : IQueryHandler<ObtenerBancosQuery, Result<PagedResult<BancoDto>>>
    {
        public async Task<Result<PagedResult<BancoDto>>> HandleAsync(
            ObtenerBancosQuery query, CancellationToken cancellationToken = default) =>
            await bancos.ListarAsync(query.Buscar, query.ToPageRequest(), cancellationToken);
    }
}
