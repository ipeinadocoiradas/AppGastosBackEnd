namespace AppGestionGastos.Domain.Movimientos;

public interface ITipoMovimientoRepository
{
    Task<TipoMovimiento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
