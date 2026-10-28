using AppGestionGastos.Application.Abstractions.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence;

/// <summary>
/// Despacho coordinado de eventos de dominio: antes de persistir, recoge los eventos de las entidades
/// rastreadas y ejecuta sus handlers.
/// Se repite hasta que no quedan eventos y entonces se hace un único SaveChanges, por lo que el
/// agregado original y los cambios de los handlers se confirman en la misma transacción.
/// Despacho después de persistir, recoge los enventos y ejecuta sus hadlers.
/// Pendiente implementar Outbox (tabla OutboxMessages) para garantizar que no se pierdan eventos después del commit
/// </summary>
internal sealed class UnitOfWork(AppDbContext context, 
    IDomainEventDispatcher domainEventDispatcher,
    IPostCommitEventDispatcher postCommitDispatcher) : IUnitOfWork
{
    public async Task<Result> CommitAsync(CancellationToken cancellationToken = default)
    {

        var domainEvents = new List<IDomainEvent>();

        var dispatchResult = await DispatchDomainEventsAsync(domainEvents, cancellationToken);
        if (dispatchResult.IsFailure) return dispatchResult;

        await context.SaveChangesAsync(cancellationToken);
        
        await postCommitDispatcher.DispatchAsync(domainEvents, CancellationToken.None);

        return Result.Success();
    }

    private async Task<Result> DispatchDomainEventsAsync(List<IDomainEvent> dispatched, CancellationToken cancellationToken)
    {
        while (true)
        {
            var entities = context.ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .Where(entity => entity.DomainEvents.Count > 0)
                .ToList();

            if (entities.Count == 0) return Result.Success();

            var domainEvents = entities.SelectMany(entity => entity.DomainEvents).ToList();
            entities.ForEach(entity => entity.ClearDomainEvents());
            dispatched.AddRange(domainEvents);

            var result = await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
            if (result.IsFailure) return result;
        }
    }
}
