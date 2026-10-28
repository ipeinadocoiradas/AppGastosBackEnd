
namespace AppGestionGastos.Application.Features.Cuentas
{
    public sealed record CuentaDto(Guid Id, string NumeroCuenta, decimal SaldoActual, Guid BancoId, string Banco, Guid TipoCuentaId, string TipoCuenta);
}
