
using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{
    internal sealed class BancoSeeder: IDataSeeder
    {
        private static readonly (string Nombre, string Descripcion)[] Datos =
        [
          ("CaixaBank", "2100"),
          ("IberCaja", "2085"),
          ("ING Direct", "1465"),
        ];

        public async Task SeedAsync(AppDbContext context, CancellationToken ct)
        {
            var existentes = (await context.Bancos
                .Select(t => t.Nombre)
                .AsAsyncEnumerable()
                .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var (nombre, codigo) in Datos.Where(d => !existentes.Contains(d.Nombre)))
            {
                var result = Banco.Crear(nombre, codigo);
                if (result.IsFailure)
                    throw new InvalidOperationException($"Seed inválido '{nombre}': {result.Error.Description}");

                context.Bancos.Add(result.Value);
            }

            await context.SaveChangesAsync(ct);
        }
    }
}
