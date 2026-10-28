

using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.TiposMovimiento.Obtener
{
    /// <param name="Buscar">Texto opcional: filtra por nombre.</param>
    public sealed record ObtenerTiposMovimientoQuery(
        int Page = 1,
        int PageSize = PageRequest.DefaultPageSize,
        string? Buscar = null) : IPagedQuery<TipoMovimientoDto>;
}
