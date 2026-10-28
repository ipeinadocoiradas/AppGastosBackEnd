using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Movimientos.Obtener
{
    /// <summary>Movimientos de las cuentas del usuario autenticado, del más reciente al más antiguo.</summary>
    public sealed record ObtenerMovimientosQuery(
        int Page = 1,
        int PageSize = PageRequest.DefaultPageSize,
        Guid? CuentaId = null,
        Guid? CategoriaId = null,
        Guid? TipoMovimientoId = null,
        DateTimeOffset? Desde = null,
        DateTimeOffset? Hasta = null) : IPagedQuery<MovimientoItemDto>;
}
