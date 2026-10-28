using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Categorias.Events;


namespace AppGestionGastos.Application.Features.Categorias.EventHandlers
{
    internal sealed class CategoriaCacheInvalidationHandler(ICatalogCacheInvalidator cache)
     : IPostCommitDomainEventHandler<CategoriaCreadaEvent>,
       IPostCommitDomainEventHandler<CategoriaActualizadaEvent>,
       IPostCommitDomainEventHandler<CategoriaEliminadaEvent>
    {
        public Task HandleAsync(CategoriaCreadaEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(CategoriaActualizadaEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(CategoriaEliminadaEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        private Task Invalidate(CancellationToken cancellationToken) =>
            cache.InvalidateAsync(CatalogCacheTags.Categorias, cancellationToken);
    }
}
