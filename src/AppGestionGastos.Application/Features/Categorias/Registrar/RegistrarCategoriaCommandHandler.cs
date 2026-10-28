using AppGestionGastos.Domain.Categorias;

namespace AppGestionGastos.Application.Features.Categorias.Crear;

internal sealed class RegistrarCategoriaCommandHandler(
    ICategoriaRepository categorias, 
    IUnitOfWork unitOfWork) : ICommandHandler<RegistrarCategoriaCommand, Result<CategoriaDto>>
{
    public async Task<Result<CategoriaDto>> HandleAsync(RegistrarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        if (await categorias.ExisteNombreAsync(command.Nombre.Trim(), cancellationToken))
        {
            return CategoriaErrors.NombreYaRegistrado;
        }

        var categoriaResult = Categoria.Crear(command.Nombre, command.Descripcion);
        if (categoriaResult.IsFailure) return categoriaResult.Error;

        var categoria = categoriaResult.Value;
        categorias.Add(categoria);

        var commit = await unitOfWork.CommitAsync(cancellationToken);
        if (commit.IsFailure) return commit.Error;

        return new CategoriaDto(categoria.Id, categoria.Nombre, categoria.Descripcion);
    }
}