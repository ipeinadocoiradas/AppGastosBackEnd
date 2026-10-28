using AppGestionGastos.Application.Common.Pagination;

namespace AppGestionGastos.Application.Features.Bancos.Obtener
{
    // <param name="Buscar">Texto opcional: filtra por nombre o código institucional.</param>
    public sealed record ObtenerBancosQuery(
        int Page = 1,
        int PageSize = PageRequest.DefaultPageSize,
        string? Buscar = null) : IPagedQuery<BancoDto>;
}
