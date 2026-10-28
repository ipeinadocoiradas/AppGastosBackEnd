

namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{
    internal interface IDataSeeder
    {
        Task SeedAsync(AppDbContext context, CancellationToken ct);
    }
}
