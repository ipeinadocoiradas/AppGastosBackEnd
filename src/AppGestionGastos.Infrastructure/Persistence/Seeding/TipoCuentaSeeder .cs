using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{
    internal sealed class TipoCuentaSeeder : IDataSeeder
    {
        private static readonly (string Nombre, string Descripcion)[] Datos =
        [
            ("Ahorros", "Cuenta de ahorro"),
            ("Corriente", "Cuenta para la operativa diaria"),
            ("Nómina", "Cuenta de abono de salario")
        ];

        public async Task SeedAsync(AppDbContext context, CancellationToken ct)
        {
            var existentes = (await context.TiposCuenta
                .Select(t => t.Nombre)
                .AsAsyncEnumerable() 
                .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var (nombre, descripcion) in Datos.Where(d => !existentes.Contains(d.Nombre)))
            {
                var result = TipoCuenta.Crear(nombre, descripcion);
                if (result.IsFailure)
                    throw new InvalidOperationException($"Seed inválido '{nombre}': {result.Error.Description}");

                context.TiposCuenta.Add(result.Value);
            }

            await context.SaveChangesAsync(ct);
        }
    }
}
