using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Cuentas.Events;


namespace AppGestionGastos.Application.Features.Bancos.EventHandlers
{
    internal sealed class BancoCreadoEventHandler(ICatalogCacheInvalidator cache)
     : IPostCommitDomainEventHandler<BancoCreadoEvent>
    {
        public Task HandleAsync(BancoCreadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            cache.InvalidateAsync(CatalogCacheTags.Bancos, cancellationToken);
    }
}
