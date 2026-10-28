using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{
    internal static class DatabaseSeeder
    {
        private static readonly IDataSeeder[] Seeders =
            [            
            new TipoCuentaSeeder(),
            new CategoriaSeeder(),
            new BancoSeeder(),
            ];

        public static async Task RunAsync(DbContext context, CancellationToken ct)
        {
            var db = (AppDbContext)context;
            foreach (var seeder in Seeders) await seeder.SeedAsync(db, ct);
        }
    }
}
