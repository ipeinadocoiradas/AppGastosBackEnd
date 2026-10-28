using AppGestionGastos.Domain.Categorias;


namespace AppGestionGastos.Application.Features.Categorias.Eliminar
{
    internal sealed class EliminarCategoriaCommandHandler(ICategoriaRepository categorias, IUnitOfWork unitOfWork)
     : ICommandHandler<EliminarCategoriaCommand, Result>
    {
        public async Task<Result> HandleAsync(
            EliminarCategoriaCommand command, CancellationToken cancellationToken = default)
        {
            var categoria = await categorias.GetByIdAsync(command.Id, cancellationToken);
            if (categoria is null) return CategoriaErrors.NoEncontrada;

            if (await categorias.EstaEnUsoAsync(categoria.Id, cancellationToken)) return CategoriaErrors.EnUso;

            categoria.Eliminar();
            categorias.Remove(categoria);

            return await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
