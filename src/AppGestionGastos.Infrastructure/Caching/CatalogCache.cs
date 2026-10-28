using AppGestionGastos.Application.Common.Pagination;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;


namespace AppGestionGastos.Infrastructure.Caching
{

    /// <summary>
    /// Lógica común de caché de catálogos paginados: construye la clave (página, tamaño y búsqueda),
    /// etiqueta la entrada con el nombre del catálogo y aplica los tiempos configurados.
    /// </summary>
    internal sealed class CatalogCache(
        HybridCache cache,
        IServiceScopeFactory scopeFactory,
        IOptionsMonitor<CatalogCacheOptions> optionsMonitor)
    {
        public ValueTask<TResult> GetOrCreateAsync<TRepository, TResult>(
            string tag,
            PageRequest page,
            string? buscar,
            Func<TRepository, CancellationToken, Task<TResult>> query,
            CancellationToken cancellationToken)
            where TRepository : notnull
        {
            var options = optionsMonitor.CurrentValue;
            var tieneBusqueda = !string.IsNullOrWhiteSpace(buscar);

            if (!options.Enabled || (tieneBusqueda && !options.CacheSearches))
            {
                return new ValueTask<TResult>(ExecuteAsync(query, cancellationToken));
            }

            return cache.GetOrCreateAsync(
                BuildKey(tag, page, buscar),
                token => new ValueTask<TResult>(ExecuteAsync(query, token)),
                new HybridCacheEntryOptions
                {
                    Expiration = options.Expiration,
                    LocalCacheExpiration = options.LocalCacheExpiration
                },
                [tag],
                cancellationToken);
        }

        /// <summary>
        /// Ejecuta la consulta en un ámbito propio: HybridCache comparte una única ejecución entre
        /// llamadas concurrentes, y esa ejecución no debe depender del DbContext de una petición
        /// concreta (que podría terminar antes).
        /// </summary>
        private async Task<TResult> ExecuteAsync<TRepository, TResult>(
            Func<TRepository, CancellationToken, Task<TResult>> query,
            CancellationToken cancellationToken)
            where TRepository : notnull
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var repository = scope.ServiceProvider.GetRequiredService<TRepository>();

            return await query(repository, cancellationToken);
        }

        private static string BuildKey(string tag, PageRequest page, string? buscar)
        {
            var busqueda = string.IsNullOrWhiteSpace(buscar)
                ? "-"
                : Uri.EscapeDataString(buscar.Trim().ToLowerInvariant());

            return $"catalog:{tag}:p{page.Page}:s{page.PageSize}:q:{busqueda}";
        }
    }
}
