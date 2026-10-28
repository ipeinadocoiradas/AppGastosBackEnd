using AppGestionGastos.Domain.Cuentas;


namespace AppGestionGastos.Application.Features.Bancos.Actualizar
{
    internal sealed class ActualizarBancoCommandHandler(IBancoRepository bancos, IUnitOfWork unitOfWork)
    : ICommandHandler<ActualizarBancoCommand, Result<BancoDto>>
    {
        public async Task<Result<BancoDto>> HandleAsync(
            ActualizarBancoCommand command, CancellationToken cancellationToken = default)
        {
            var banco = await bancos.GetByIdAsync(command.Id, cancellationToken);
            if (banco is null) return CuentaErrors.BancoNoEncontrado;

            if (await bancos.ExisteCodigoEnOtroAsync(banco.Id, command.CodigoInstitucional.Trim(), cancellationToken))
            {
                return CuentaErrors.BancoCodigoYaRegistrado;
            }

            var resultado = banco.Actualizar(command.Nombre, command.CodigoInstitucional);
            if (resultado.IsFailure) return resultado.Error;

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new BancoDto(banco.Id, banco.Nombre, banco.CodigoInstitucional);
        }
    }
}
