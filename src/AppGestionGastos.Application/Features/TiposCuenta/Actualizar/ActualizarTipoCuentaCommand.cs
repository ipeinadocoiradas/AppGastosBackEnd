
namespace AppGestionGastos.Application.Features.TiposCuenta.Actualizar
{
    public sealed record ActualizarTipoCuentaCommand(Guid Id, string Nombre, string? Descripcion) : ICommand<Result<TipoCuentaDto>>;
}
