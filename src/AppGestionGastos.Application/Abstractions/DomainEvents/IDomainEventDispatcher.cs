namespace AppGestionGastos.Application.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    /// <summary>Ejecuta los handlers de cada evento; se detiene en el primer fallo.</summary>
    Task<Result> DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
