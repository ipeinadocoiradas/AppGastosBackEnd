using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class CuentaRepository(AppDbContext context) : ICuentaRepository
{
    public Task<bool> ExisteNumeroAsync(Guid bancoId, string numeroCuenta, CancellationToken cancellationToken = default) =>
    context.Cuentas.AsNoTracking().AnyAsync(
        c => c.BancoId == bancoId && c.NumeroCuenta == numeroCuenta, cancellationToken);

    public Task<Cuenta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Cuentas.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public void Add(Cuenta cuenta) => context.Cuentas.Add(cuenta);

    public Task<bool> ExisteNumeroEnOtraCuentaAsync(
    Guid cuentaId, Guid bancoId, string numeroCuenta, CancellationToken cancellationToken = default) =>
    context.Cuentas.AsNoTracking().AnyAsync(
        c => c.Id != cuentaId && c.BancoId == bancoId && c.NumeroCuenta == numeroCuenta,
        cancellationToken);
}
