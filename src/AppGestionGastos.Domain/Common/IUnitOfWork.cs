namespace AppGestionGastos.Domain.Common;

public interface IUnitOfWork
{
    /// <summary>
    /// Despacha los eventos de dominio pendientes y persiste todos los cambios en una única transacción.
    /// Devuelve fallo si algún handler de eventos falla (en ese caso no se persiste nada).
    /// </summary>
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
