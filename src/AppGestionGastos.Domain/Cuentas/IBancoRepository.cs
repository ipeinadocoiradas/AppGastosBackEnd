namespace AppGestionGastos.Domain.Cuentas;

public interface IBancoRepository
{
    Task<Banco?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExisteCodigoAsync(string codigoInstitucional, CancellationToken cancellationToken = default);
    void Add(Banco banco);
    Task<bool> ExisteCodigoEnOtroAsync(Guid id, string codigoInstitucional, CancellationToken cancellationToken = default);
    Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(Banco banco);
}