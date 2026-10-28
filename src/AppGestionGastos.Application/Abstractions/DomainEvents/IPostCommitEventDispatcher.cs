namespace AppGestionGastos.Application.Abstractions.DomainEvents;

public interface IPostCommitEventDispatcher
{
    /// <summary>Ejecuta todos los handlers (sin detenerse en el primer fallo). Devuelve fallo si alguno falló.</summary>
    Task<Result> DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}