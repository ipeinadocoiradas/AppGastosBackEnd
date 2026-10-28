namespace AppGestionGastos.Domain.Categorias;

public interface ICategoriaRepository
{
    Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default);
    void Add(Categoria categoria);
    Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExisteNombreEnOtraAsync(Guid id, string nombre, CancellationToken cancellationToken = default);
    Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(Categoria categoria);
}
