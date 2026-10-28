using AppGestionGastos.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Repositories;

internal sealed class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public Task<bool> ExisteEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        context.Usuarios.AsNoTracking().AnyAsync(u => u.Email == email, cancellationToken);

    public Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default) =>
        context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public void Add(Usuario usuario) => context.Usuarios.Add(usuario);

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
    context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Cuentas.AsNoTracking().AnyAsync(c => c.UsuarioId == id, cancellationToken);

    public void Remove(Usuario usuario) => context.Usuarios.Remove(usuario);
}
