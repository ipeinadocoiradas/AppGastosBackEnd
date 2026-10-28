using AppGestionGastos.Domain.Cuentas;


namespace AppGestionGastos.Application.Features.TiposCuenta.Eliminar
{
    internal sealed class EliminarTipoCuentaCommandHandler(ITipoCuentaRepository tiposCuenta, IUnitOfWork unitOfWork)
       : ICommandHandler<EliminarTipoCuentaCommand, Result>
    {
        public async Task<Result> HandleAsync(
            EliminarTipoCuentaCommand command, CancellationToken cancellationToken = default)
        {
            var tipoCuenta = await tiposCuenta.GetByIdAsync(command.Id, cancellationToken);
            if (tipoCuenta is null) return CuentaErrors.TipoCuentaNoEncontrado;

            if (await tiposCuenta.EstaEnUsoAsync(tipoCuenta.Id, cancellationToken)) return CuentaErrors.TipoCuentaEnUso;

            tipoCuenta.Eliminar();
            tiposCuenta.Remove(tipoCuenta);

            return await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
