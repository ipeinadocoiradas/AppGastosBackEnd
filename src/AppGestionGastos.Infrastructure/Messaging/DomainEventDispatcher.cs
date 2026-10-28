using System.Collections.Concurrent;
using AppGestionGastos.Application.Abstractions.DomainEvents;

namespace AppGestionGastos.Infrastructure.Messaging;

internal sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, DomainEventInvoker> Invokers = new();

    public async Task<Result> DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var invoker = Invokers.GetOrAdd(
                domainEvent.GetType(),
                static eventType => (DomainEventInvoker)Activator.CreateInstance(
                    typeof(DomainEventInvoker<>).MakeGenericType(eventType))!);

            var result = await invoker.InvokeAsync(domainEvent, serviceProvider, cancellationToken);
            if (result.IsFailure) return result;
        }

        return Result.Success();
    }
}

internal abstract class DomainEventInvoker
{
    public abstract Task<Result> InvokeAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

internal sealed class DomainEventInvoker<TEvent> : DomainEventInvoker where TEvent : IDomainEvent
{
    public override async Task<Result> InvokeAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        foreach (var handler in serviceProvider.GetServices<IDomainEventHandler<TEvent>>())
        {
            var result = await handler.HandleAsync((TEvent)domainEvent, cancellationToken);
            if (result.IsFailure) return result;
        }

        return Result.Success();
    }
}
