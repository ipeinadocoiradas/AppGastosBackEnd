using Microsoft.Extensions.DependencyInjection;

namespace AppGestionGastos.Infrastructure.Persistence.Seeding;

public static class SeedingExtensions
{
    public static async Task SeedDevelopmentDataAsync(this IServiceProvider services, CancellationToken ct = default)
    {
        await using var scope = services.CreateAsyncScope();

        var seeder = ActivatorUtilities.CreateInstance<DevelopmentDataSeeder>(scope.ServiceProvider);
        await seeder.SeedAsync(ct);
    }
}