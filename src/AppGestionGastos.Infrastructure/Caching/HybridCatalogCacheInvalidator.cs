using AppGestionGastos.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;


namespace AppGestionGastos.Infrastructure.Caching
{

    internal sealed class HybridCatalogCacheInvalidator(
        HybridCache cache,
        ILogger<HybridCatalogCacheInvalidator> logger) : ICatalogCacheInvalidator
    {
        public async Task InvalidateAsync(string tag, CancellationToken cancellationToken = default)
        {
            await cache.RemoveByTagAsync(tag, cancellationToken);

            logger.LogInformation("Caché de catálogo eliminada: {Tag}", tag);
        }
    }
}
