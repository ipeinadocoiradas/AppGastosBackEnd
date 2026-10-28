namespace AppGestionGastos.Domain.Usuarios;

public interface IUsuarioRepository
{
    Task<bool> ExisteEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    void Add(Usuario usuario);
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(Usuario usuario);
}
