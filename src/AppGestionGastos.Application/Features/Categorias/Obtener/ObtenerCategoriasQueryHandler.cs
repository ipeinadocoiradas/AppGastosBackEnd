using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Categorias.Obtener
{
    internal sealed class ObtenerCategoriasQueryHandler(ICategoriaReadRepository categorias)
      : IQueryHandler<ObtenerCategoriasQuery, Result<PagedResult<CategoriaDto>>>
    {
        public async Task<Result<PagedResult<CategoriaDto>>> HandleAsync(
            ObtenerCategoriasQuery query, CancellationToken cancellationToken = default) =>
            await categorias.ListarAsync(query.Buscar, query.ToPageRequest(), cancellationToken);
    }
}
