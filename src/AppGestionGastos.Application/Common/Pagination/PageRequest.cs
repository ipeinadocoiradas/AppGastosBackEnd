namespace AppGestionGastos.Application.Common.Pagination
{
    /// <summary>Petición de página normalizada (defensa en profundidad además del validador).</summary>
    public readonly record struct PageRequest
    {
        public const int DefaultPageSize = 20;
        public const int MaxPageSize = 100;

        public PageRequest(int page, int pageSize)
        {
            Page = Math.Max(1, page);
            PageSize = Math.Clamp(pageSize, 1, MaxPageSize);
        }

        public int Page { get; }
        public int PageSize { get; }
        public int Skip => (Page - 1) * PageSize;
    }
}
