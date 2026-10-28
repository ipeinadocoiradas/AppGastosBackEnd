using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Categorias.Events;


namespace AppGestionGastos.Application.Features.Categorias.EventHandlers
{
    internal sealed class CategoriaCreadaEventHandler(ICatalogCacheInvalidator cache)
     : IPostCommitDomainEventHandler<CategoriaCreadaEvent>
    {
        public Task HandleAsync(CategoriaCreadaEvent domainEvent, CancellationToken cancellationToken = default) =>
            cache.InvalidateAsync(CatalogCacheTags.Categorias, cancellationToken);
    }
}
