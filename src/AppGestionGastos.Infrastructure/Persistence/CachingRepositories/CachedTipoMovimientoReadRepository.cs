using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposMovimiento;
using AppGestionGastos.Infrastructure.Caching;
using AppGestionGastos.Infrastructure.Persistence.ReadRepositories;


namespace AppGestionGastos.Infrastructure.Persistence.CachingRepositories
{
    internal sealed class CachedTipoMovimientoReadRepository(CatalogCache cache) : ITipoMovimientoReadRepository
    {
        public async Task<PagedResult<TipoMovimientoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default) =>
            await cache.GetOrCreateAsync<TipoMovimientoReadRepository, PagedResult<TipoMovimientoDto>>(
                CatalogCacheTags.TiposMovimiento, page, buscar,
                (repository, token) => repository.ListarAsync(buscar, page, token),
                cancellationToken);
    }
}
