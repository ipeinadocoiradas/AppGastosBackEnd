

namespace AppGestionGastos.Application.Features.Categorias.Eliminar
{
    public sealed record EliminarCategoriaCommand(Guid Id) : ICommand<Result>;
}
