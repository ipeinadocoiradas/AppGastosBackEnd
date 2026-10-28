

namespace AppGestionGastos.Application.Features.TiposCuenta.Eliminar
{
    public sealed record EliminarTipoCuentaCommand(Guid Id) : ICommand<Result>;
}
