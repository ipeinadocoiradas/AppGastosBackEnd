using AppGestionGastos.Application.Common.Pagination;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.Pagination
{
    public static class QueryablePaginationExtensions
    {
        /// <summary>
        /// Pagina una consulta ya ORDENADA y proyecta solo la página solicitada.
        /// Exigir <see cref="IOrderedQueryable{T}"/> garantiza, en compilación, un orden determinista
        /// (sin él, Skip/Take puede repetir u omitir filas entre páginas).
        /// </summary>
        public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
            this IOrderedQueryable<TSource> source,
            PageRequest page,
            Expression<Func<TSource, TResult>> selector,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await source.CountAsync(cancellationToken);
            if (totalCount == 0) return PagedResult<TResult>.Empty(page);

            var items = await source
                .Skip(page.Skip)
                .Take(page.PageSize)
                .Select(selector)
                .ToListAsync(cancellationToken);

            return new PagedResult<TResult>(items, page.Page, page.PageSize, totalCount);
        }
    }
}
