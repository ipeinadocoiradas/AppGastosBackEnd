
namespace AppGestionGastos.Application.Features.Bancos.Eliminar
{
    public sealed record EliminarBancoCommand(Guid Id) : ICommand<Result>;
}
