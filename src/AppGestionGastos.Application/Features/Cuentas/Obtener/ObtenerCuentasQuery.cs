using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Cuentas.Obtener
{
    public sealed record ObtenerCuentasQuery(int Page = 1,
      int PageSize = PageRequest.DefaultPageSize, string? Buscar = null) : IPagedQuery<CuentaDto>;
}
