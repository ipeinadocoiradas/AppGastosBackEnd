using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.Movimientos
{
    public sealed record MovimientoFiltro(
     Guid UsuarioId,
     Guid? CuentaId,
     Guid? CategoriaId,
     Guid? TipoMovimientoId,
     DateTimeOffset? Desde,
     DateTimeOffset? Hasta);

    public interface IMovimientoReadRepository
    {
        Task<PagedResult<MovimientoItemDto>> ListarAsync(
            MovimientoFiltro filtro, PageRequest page, CancellationToken cancellationToken = default);
    }
}
