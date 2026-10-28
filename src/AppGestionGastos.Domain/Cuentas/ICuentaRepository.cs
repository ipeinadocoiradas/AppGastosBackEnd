namespace AppGestionGastos.Domain.Cuentas;

public interface ICuentaRepository
{
    Task<bool> ExisteNumeroAsync(Guid bancoId, string numeroCuenta, CancellationToken cancellationToken = default);
    Task<Cuenta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Cuenta cuenta);
    Task<bool> ExisteNumeroEnOtraCuentaAsync(Guid cuentaId, Guid bancoId, string numeroCuenta, CancellationToken cancellationToken = default);
}
