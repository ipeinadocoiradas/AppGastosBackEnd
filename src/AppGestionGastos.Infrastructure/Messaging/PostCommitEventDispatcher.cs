using System.Collections.Concurrent;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using Microsoft.Extensions.Logging;

namespace AppGestionGastos.Infrastructure.Messaging;

internal sealed class PostCommitEventDispatcher(
    IServiceProvider serviceProvider,
    ILogger<PostCommitEventDispatcher> logger) : IPostCommitEventDispatcher
{
    private static readonly ConcurrentDictionary<Type, PostCommitInvoker> Invokers = new();

    public async Task<Result> DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        var failures = 0;

        foreach (var domainEvent in domainEvents)
        {
            var invoker = Invokers.GetOrAdd(
                domainEvent.GetType(),
                static type => (PostCommitInvoker)Activator.CreateInstance(
                    typeof(PostCommitInvoker<>).MakeGenericType(type))!);

            failures += await invoker.InvokeAsync(domainEvent, serviceProvider, logger, cancellationToken);
        }

        if (failures > 0)
        {
            return Error.Failure("PostCommit.HandlersFallidos", $"Fallaron {failures} handlers post-commit.");
        }

        return Result.Success();
    }
}

internal abstract class PostCommitInvoker
{
    /// <returns>Número de handlers que fallaron.</returns>
    public abstract Task<int> InvokeAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, ILogger logger, CancellationToken cancellationToken);
}

internal sealed class PostCommitInvoker<TEvent> : PostCommitInvoker where TEvent : IDomainEvent
{
    public override async Task<int> InvokeAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, ILogger logger, CancellationToken cancellationToken)
    {
        var failures = 0;

        foreach (var handler in serviceProvider.GetServices<IPostCommitDomainEventHandler<TEvent>>())
        {
            try
            {
                await handler.HandleAsync((TEvent)domainEvent, cancellationToken);
            }
            catch (Exception ex)
            {
                failures++;
                logger.LogError(
                    ex,
                    "Falló el handler post-commit {Handler} para el evento {EventType} ({EventId})",
                    handler.GetType().Name, typeof(TEvent).Name, domainEvent.EventId);
            }
        }

        return failures;
    }
}