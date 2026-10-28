
namespace AppGestionGastos.Application.Features.Bancos.Actualizar
{
    /// <param name="Id">Se toma de la ruta; el valor del cuerpo se ignora.</param>
    public sealed record ActualizarBancoCommand(Guid Id, string Nombre, string CodigoInstitucional) : ICommand<Result<BancoDto>>;
}
