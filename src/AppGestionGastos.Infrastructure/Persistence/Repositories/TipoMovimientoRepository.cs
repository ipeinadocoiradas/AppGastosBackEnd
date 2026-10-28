using AppGestionGastos.Domain.Movimientos;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class TipoMovimientoRepository(AppDbContext context) : ITipoMovimientoRepository
{
    public Task<TipoMovimiento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.TiposMovimiento.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
}
