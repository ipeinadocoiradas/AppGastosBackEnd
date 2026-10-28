using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class BancoRepository(AppDbContext context) : IBancoRepository
{
    public Task<Banco?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
    context.Bancos.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public Task<bool> ExisteCodigoAsync(string codigoInstitucional, CancellationToken cancellationToken = default) =>
        context.Bancos.AsNoTracking().AnyAsync(b => b.CodigoInstitucional == codigoInstitucional, cancellationToken);

    public void Add(Banco banco) => context.Bancos.Add(banco);

    public Task<bool> ExisteCodigoEnOtroAsync(
        Guid id, string codigoInstitucional, CancellationToken cancellationToken = default) =>
        context.Bancos.AsNoTracking()
            .AnyAsync(b => b.Id != id && b.CodigoInstitucional == codigoInstitucional, cancellationToken);

    public Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Cuentas.AsNoTracking().AnyAsync(c => c.BancoId == id, cancellationToken);

    public void Remove(Banco banco) => context.Bancos.Remove(banco);
}