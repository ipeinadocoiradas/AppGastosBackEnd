
namespace AppGestionGastos.Application.Features.Cuentas.Actualizar
{
    public sealed record ActualizarCuentaCommand(Guid Id, Guid BancoId, Guid TipoCuentaId, string NumeroCuenta) : ICommand<Result<CuentaDto>>;
}
