using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Categorias;
using AppGestionGastos.Infrastructure.Caching;
using AppGestionGastos.Infrastructure.Persistence.ReadRepositories;


namespace AppGestionGastos.Infrastructure.Persistence.CachingRepositories
{
    internal sealed class CachedCategoriaReadRepository(CatalogCache cache) : ICategoriaReadRepository
    {
        public async Task<PagedResult<CategoriaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default) =>
            await cache.GetOrCreateAsync<CategoriaReadRepository, PagedResult<CategoriaDto>>(
                CatalogCacheTags.Categorias, page, buscar,
                (repository, token) => repository.ListarAsync(buscar, page, token),
                cancellationToken);
    }
}
