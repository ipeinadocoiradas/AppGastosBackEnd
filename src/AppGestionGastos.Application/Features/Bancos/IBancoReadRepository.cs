using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Bancos
{
    public interface IBancoReadRepository
    {
        Task<PagedResult<BancoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
