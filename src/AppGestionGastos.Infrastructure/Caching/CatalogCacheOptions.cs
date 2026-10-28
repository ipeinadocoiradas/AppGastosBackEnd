

namespace AppGestionGastos.Infrastructure.Caching
{
    public sealed class CatalogCacheOptions
    {
        public const string SectionName = "CatalogCache";

        /// <summary>Interruptor general: si es false, las consultas van siempre a la base de datos.</summary>
        public bool Enabled { get; init; } = true;

        /// <summary>Duración total de la entrada (y de la caché distribuida L2, si la hay).</summary>
        public TimeSpan Expiration { get; init; } = TimeSpan.FromMinutes(30);

        /// <summary>Duración en la memoria local (L1) de cada instancia de la API.</summary>
        public TimeSpan LocalCacheExpiration { get; init; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Si es false (recomendado) las búsquedas por texto libre no se cachean, para evitar
        /// que valores arbitrarios llenen la caché de claves.
        /// </summary>
        public bool CacheSearches { get; init; } = false;
    }
}
