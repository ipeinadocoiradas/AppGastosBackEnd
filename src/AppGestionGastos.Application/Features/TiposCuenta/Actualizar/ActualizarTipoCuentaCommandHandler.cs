using AppGestionGastos.Domain.Cuentas;

namespace AppGestionGastos.Application.Features.TiposCuenta.Actualizar
{
    internal sealed class ActualizarTipoCuentaCommandHandler(ITipoCuentaRepository tiposCuenta, IUnitOfWork unitOfWork)
      : ICommandHandler<ActualizarTipoCuentaCommand, Result<TipoCuentaDto>>
    {
        public async Task<Result<TipoCuentaDto>> HandleAsync(
            ActualizarTipoCuentaCommand command, CancellationToken cancellationToken = default)
        {
            var tipoCuenta = await tiposCuenta.GetByIdAsync(command.Id, cancellationToken);
            if (tipoCuenta is null) return CuentaErrors.TipoCuentaNoEncontrado;

            if (await tiposCuenta.ExisteNombreEnOtroAsync(tipoCuenta.Id, command.Nombre.Trim(), cancellationToken))
            {
                return CuentaErrors.TipoCuentaNombreYaRegistrado;
            }

            var resultado = tipoCuenta.Actualizar(command.Nombre, command.Descripcion);
            if (resultado.IsFailure) return resultado.Error;

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new TipoCuentaDto(tipoCuenta.Id, tipoCuenta.Nombre, tipoCuenta.Descripcion);
        }
    }
}
