
namespace AppGestionGastos.Application.Features.Cuentas.Registrar
{
    public sealed record RegistrarCuentaCommand(
        string NumeroCuenta,
        decimal SaldoActual,
        Guid BancoId,
        Guid TipoCuentaId): ICommand<Result<CuentaDto>>;    
}
