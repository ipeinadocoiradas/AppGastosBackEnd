namespace AppGestionGastos.Application.Abstractions.DomainEvents;

/// <summary>
/// Reacciona a un evento una vez confirmada la transacción. Debe ser idempotente.
/// Si lanza una excepción, el fallo se registra pero los datos ya están persistidos.
/// </summary>
public interface IPostCommitDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}