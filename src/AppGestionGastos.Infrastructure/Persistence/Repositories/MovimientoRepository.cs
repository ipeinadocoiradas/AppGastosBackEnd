using AppGestionGastos.Domain.Movimientos;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class MovimientoRepository(AppDbContext context) : IMovimientoRepository
{
    public void Add(Movimiento movimiento) => context.Movimientos.Add(movimiento);
}
