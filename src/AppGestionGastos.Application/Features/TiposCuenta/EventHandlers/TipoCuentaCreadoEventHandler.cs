using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Cuentas.Events;

namespace AppGestionGastos.Application.Features.TiposCuenta.EventHandlers
{

    internal sealed class TipoCuentaCreadoEventHandler(ICatalogCacheInvalidator cache)
        : IPostCommitDomainEventHandler<TipoCuentaCreadoEvent>
    {
        public Task HandleAsync(TipoCuentaCreadoEvent domainEvent, CancellationToken cancellationToken = default) =>
            cache.InvalidateAsync(CatalogCacheTags.TiposCuenta, cancellationToken);
    }
}
