namespace AppGestionGastos.Application.Common.Pagination
{
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public static PagedResult<T> Empty(PageRequest page) => new([], page.Page, page.PageSize, 0);
    }
}
