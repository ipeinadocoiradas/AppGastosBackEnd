

namespace AppGestionGastos.Application.Features.Categorias.Actualizar
{
    public sealed record ActualizarCategoriaCommand(Guid Id, string Nombre, string? Descripcion) : ICommand<Result<CategoriaDto>>;
}
