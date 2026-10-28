using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposCuenta;
using AppGestionGastos.Infrastructure.Caching;
using AppGestionGastos.Infrastructure.Persistence.ReadRepositories;


namespace AppGestionGastos.Infrastructure.Persistence.CachingRepositories
{

    internal sealed class CachedTipoCuentaReadRepository(CatalogCache cache) : ITipoCuentaReadRepository
    {
        public async Task<PagedResult<TipoCuentaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default) =>
            await cache.GetOrCreateAsync<TipoCuentaReadRepository, PagedResult<TipoCuentaDto>>(
                CatalogCacheTags.TiposCuenta, page, buscar,
                (repository, token) => repository.ListarAsync(buscar, page, token),
                cancellationToken);
    }
}
