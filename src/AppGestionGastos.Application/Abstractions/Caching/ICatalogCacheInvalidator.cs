
namespace AppGestionGastos.Application.Abstractions.Caching
{
    public interface ICatalogCacheInvalidator
    {
        /// <summary>Invalida todas las entradas (todas las páginas y búsquedas) de un catálogo.</summary>
        Task InvalidateAsync(string tag, CancellationToken cancellationToken = default);
    }
}
