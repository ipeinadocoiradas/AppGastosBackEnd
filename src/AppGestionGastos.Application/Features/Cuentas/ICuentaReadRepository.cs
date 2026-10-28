using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.Cuentas
{
    public interface ICuentaReadRepository
    {
        Task<PagedResult<CuentaDto>> ListarAsync(
          Guid usuarioId, string? buscar, PageRequest page, CancellationToken cancellationToken = default);
    }
}
