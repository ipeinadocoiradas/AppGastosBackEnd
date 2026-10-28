using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Categorias.Obtener
{
    public sealed record ObtenerCategoriasQuery(
      int Page = 1,
      int PageSize = PageRequest.DefaultPageSize,
      string? Buscar = null) : IPagedQuery<CategoriaDto>;
}
