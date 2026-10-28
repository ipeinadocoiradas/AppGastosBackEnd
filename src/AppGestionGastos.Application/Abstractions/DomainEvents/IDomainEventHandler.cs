namespace AppGestionGastos.Application.Abstractions.DomainEvents;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task<Result> HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
