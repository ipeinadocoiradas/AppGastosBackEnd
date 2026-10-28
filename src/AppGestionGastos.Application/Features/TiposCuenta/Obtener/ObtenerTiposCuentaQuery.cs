using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.TiposCuenta.Obtener
{
    public sealed record ObtenerTiposCuentaQuery(
     int Page = 1,
     int PageSize = PageRequest.DefaultPageSize,
     string? Buscar = null) : IPagedQuery<TipoCuentaDto>;
}
