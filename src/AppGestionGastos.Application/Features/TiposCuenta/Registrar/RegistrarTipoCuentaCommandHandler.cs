using AppGestionGastos.Domain.Cuentas;

namespace AppGestionGastos.Application.Features.TiposCuenta.Crear;

internal sealed class RegistrarTipoCuentaCommandHandler(ITipoCuentaRepository tiposCuenta, IUnitOfWork unitOfWork)
    : ICommandHandler<RegistrarTipoCuentaCommand, Result<TipoCuentaDto>>
{
    public async Task<Result<TipoCuentaDto>> HandleAsync(RegistrarTipoCuentaCommand command, CancellationToken cancellationToken = default)
    {
        if (await tiposCuenta.ExisteNombreAsync(command.Nombre.Trim(), cancellationToken))
        {
            return CuentaErrors.TipoCuentaNombreYaRegistrado;
        }

        var tipoResult = TipoCuenta.Crear(command.Nombre, command.Descripcion);
        if (tipoResult.IsFailure) return tipoResult.Error;

        var tipoCuenta = tipoResult.Value;
        tiposCuenta.Add(tipoCuenta);

        var commit = await unitOfWork.CommitAsync(cancellationToken);
        if (commit.IsFailure) return commit.Error;

        return new TipoCuentaDto(tipoCuenta.Id, tipoCuenta.Nombre, tipoCuenta.Descripcion);
    }
}