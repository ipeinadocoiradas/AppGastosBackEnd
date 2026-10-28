using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Categorias
{
    public interface ICategoriaReadRepository
    {
        Task<PagedResult<CategoriaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
