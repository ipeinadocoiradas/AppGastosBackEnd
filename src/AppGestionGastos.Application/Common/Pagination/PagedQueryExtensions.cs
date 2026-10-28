

namespace AppGestionGastos.Application.Common.Pagination
{
    public static class PagedQueryExtensions
    {
        /// <summary>Convierte Page/PageSize de la consulta en una <see cref="PageRequest"/> normalizada.</summary>
        public static PageRequest ToPageRequest(this IPagedQuery query) => new(query.Page, query.PageSize);
    }
}
