using AppGestionGastos.Domain.Categorias;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    public Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
    context.Categorias.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Categorias.AsNoTracking().AnyAsync(c => c.Id == id, cancellationToken);

    public Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default) =>
    context.Categorias.AsNoTracking().AnyAsync(c => c.Nombre == nombre, cancellationToken);

    public void Add(Categoria categoria) => context.Categorias.Add(categoria);

    public Task<bool> ExisteNombreEnOtraAsync(
    Guid id, string nombre, CancellationToken cancellationToken = default) =>
    context.Categorias.AsNoTracking().AnyAsync(c => c.Id != id && c.Nombre == nombre, cancellationToken);

    public Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Movimientos.AsNoTracking().AnyAsync(m => m.CategoriaId == id, cancellationToken);

    public void Remove(Categoria categoria) => context.Categorias.Remove(categoria);
}
