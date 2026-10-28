

namespace AppGestionGastos.Application.Common.Pagination
{
    /// <summary>Contrato mínimo de una consulta paginada (lo usa el validador reutilizable).</summary>
    public interface IPagedQuery
    {
        int Page { get; }
        int PageSize { get; }      
    }

    /// <summary>Consulta paginada que devuelve una página de <typeparamref name="TItem"/>.</summary>
    public interface IPagedQuery<TItem> : IPagedQuery, IQuery<Result<PagedResult<TItem>>>;
}
