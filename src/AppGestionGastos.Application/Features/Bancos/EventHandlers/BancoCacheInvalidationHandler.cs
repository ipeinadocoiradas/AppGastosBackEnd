using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Cuentas.Events;

namespace AppGestionGastos.Application.Features.Bancos.EventHandlers
{
    internal sealed class BancoCacheInvalidationHandler(ICatalogCacheInvalidator cache)
      : IPostCommitDomainEventHandler<BancoCreadoEvent>,
        IPostCommitDomainEventHandler<BancoActualizadoEvent>,
        IPostCommitDomainEventHandler<BancoEliminadoEvent>
    {
        public Task HandleAsync(BancoCreadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(BancoActualizadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        public Task HandleAsync(BancoEliminadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            Invalidate(cancellationToken);

        private Task Invalidate(CancellationToken cancellationToken) => cache.InvalidateAsync(CatalogCacheTags.Bancos, cancellationToken);
    }
}
