using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Bancos;
using AppGestionGastos.Infrastructure.Caching;
using AppGestionGastos.Infrastructure.Persistence.ReadRepositories;


namespace AppGestionGastos.Infrastructure.Persistence.CachingRepositories
{
    internal sealed class CachedBancoReadRepository(CatalogCache cache) : IBancoReadRepository
    {
        public async Task<PagedResult<BancoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default) =>
            await cache.GetOrCreateAsync<BancoReadRepository, PagedResult<BancoDto>>(
                CatalogCacheTags.Bancos, page, buscar,
                (repository, token) => repository.ListarAsync(buscar, page, token),
                cancellationToken);
    }
}
