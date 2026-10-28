using AppGestionGastos.Domain.Cuentas;


namespace AppGestionGastos.Application.Features.Bancos.Eliminar
{
    internal sealed class EliminarBancoCommandHandler(IBancoRepository bancos, IUnitOfWork unitOfWork)
      : ICommandHandler<EliminarBancoCommand, Result>
    {
        public async Task<Result> HandleAsync(
            EliminarBancoCommand command, CancellationToken cancellationToken = default)
        {
            var banco = await bancos.GetByIdAsync(command.Id, cancellationToken);
            if (banco is null) return CuentaErrors.BancoNoEncontrado;

            if (await bancos.EstaEnUsoAsync(banco.Id, cancellationToken)) return CuentaErrors.BancoEnUso;

            banco.Eliminar();       // registra BancoEliminadoEvent
            bancos.Remove(banco);

            return await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
