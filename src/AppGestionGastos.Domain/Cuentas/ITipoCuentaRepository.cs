namespace AppGestionGastos.Domain.Cuentas;

public interface ITipoCuentaRepository
{
    Task<TipoCuenta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default);
    void Add(TipoCuenta tipoCuenta);
    Task<bool> ExisteNombreEnOtroAsync(Guid id, string nombre, CancellationToken cancellationToken = default);
    Task<bool> EstaEnUsoAsync(Guid id, CancellationToken cancellationToken = default);
    void Remove(TipoCuenta tipoCuenta);
}