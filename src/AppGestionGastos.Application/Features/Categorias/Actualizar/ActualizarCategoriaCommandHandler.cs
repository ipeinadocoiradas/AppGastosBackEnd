using AppGestionGastos.Domain.Categorias;


namespace AppGestionGastos.Application.Features.Categorias.Actualizar
{
    internal sealed class ActualizarCategoriaCommandHandler(ICategoriaRepository categorias, IUnitOfWork unitOfWork)
     : ICommandHandler<ActualizarCategoriaCommand, Result<CategoriaDto>>
    {
        public async Task<Result<CategoriaDto>> HandleAsync(
            ActualizarCategoriaCommand command, CancellationToken cancellationToken = default)
        {
            var categoria = await categorias.GetByIdAsync(command.Id, cancellationToken);
            if (categoria is null) return CategoriaErrors.NoEncontrada;

            if (await categorias.ExisteNombreEnOtraAsync(categoria.Id, command.Nombre.Trim(), cancellationToken))
            {
                return CategoriaErrors.NombreYaRegistrado;
            }

            var resultado = categoria.Actualizar(command.Nombre, command.Descripcion);
            if (resultado.IsFailure) return resultado.Error;

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new CategoriaDto(categoria.Id, categoria.Nombre, categoria.Descripcion);
        }
    }
}
