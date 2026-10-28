using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class TipoCuentaRepository(AppDbContext context) : ITipoCuentaRepository
{
    public Task<TipoCuenta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
    context.TiposCuenta.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default) =>
        context.TiposCuenta.AsNoTracking().AnyAsync(t => t.Nombre == nombre, cancellationToken);

    public void Add(TipoCuenta tipoCuenta) => context.TiposCuenta.Add(tipoCuenta);

    public Task<bool> ExisteNombreEnOtroAsync(
    Guid id, string nombre, CancellationToken cancellationToken = default) =>
    context.TiposCuenta.AsNoTracking().AnyAsync(t => t.Id != id && t.Nombre == nombre, cancellationToken);

    public Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Cuentas.AsNoTracking().AnyAsync(c => c.TipoCuentaId == id, cancellationToken);

    public void Remove(TipoCuenta tipoCuenta) => context.TiposCuenta.Remove(tipoCuenta);
}