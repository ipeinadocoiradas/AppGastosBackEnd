using AppGestionGastos.Domain.Cuentas;

namespace AppGestionGastos.Application.Features.Bancos.Crear;

internal sealed class RegistrarBancoCommandHandler(IBancoRepository bancos, IUnitOfWork unitOfWork)
    : ICommandHandler<RegistrarBancoCommand, Result<BancoDto>>
{
    public async Task<Result<BancoDto>> HandleAsync(RegistrarBancoCommand command, CancellationToken cancellationToken = default)
    {
        if (await bancos.ExisteCodigoAsync(command.CodigoInstitucional.Trim(), cancellationToken))
        {
            return CuentaErrors.BancoCodigoYaRegistrado;
        }

        var bancoResult = Banco.Crear(command.Nombre, command.CodigoInstitucional);
        if (bancoResult.IsFailure) return bancoResult.Error;

        var banco = bancoResult.Value;
        bancos.Add(banco);

        var commit = await unitOfWork.CommitAsync(cancellationToken);
        if (commit.IsFailure) return commit.Error;

        return new BancoDto(banco.Id, banco.Nombre, banco.CodigoInstitucional);
    }
}