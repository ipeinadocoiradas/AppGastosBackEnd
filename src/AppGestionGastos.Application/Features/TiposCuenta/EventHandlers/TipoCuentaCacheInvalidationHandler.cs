using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Cuentas.Events;


namespace AppGestionGastos.Application.Features.TiposCuenta.EventHandlers
{
    internal sealed class TipoCuentaCacheInvalidationHandler(ICatalogCacheInvalidator cache)
     : IPostCommitDomainEventHandler<TipoCuentaCreadoEvent>,
       IPostCommitDomainEventHandler<TipoCuentaActualizadoEvent>,
       IPostCommitDomainEventHandler<TipoCuentaEliminadoEvent>
    {
        public Task HandleAsync(TipoCuentaCreadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(TipoCuentaActualizadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(TipoCuentaEliminadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        private Task Invalidate(CancellationToken cancellationToken) =>
            cache.InvalidateAsync(CatalogCacheTags.TiposCuenta, cancellationToken);
    }
}
