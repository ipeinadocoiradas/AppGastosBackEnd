using AppGestionGastos.Domain.Categorias;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{
    internal sealed class CategoriaSeeder : IDataSeeder
    {
        private static readonly (string Nombre, string Descripcion)[] Datos =
        [
            ("Hogar", "Hogar"),
            ("Formación", "Formación"),
            ("Impuestos", "Impuestos"),
            ("Inmuebles", "Inmuebles"),
            ("Inversiones", "Inversiones"),
            ("Ocio", "Ocio"),
            ("Vacaciones", "Vacaciones"),
            ("Regalos", "Regalos"),
            ("Salud y Deporte", "Salud y Deporte"),
            ("Vehículo", "Vehículo"),
            ("Laboral", "Laboral"),
            ("Varios", "Varios")
        ];

        public async Task SeedAsync(AppDbContext context, CancellationToken ct)
        {
            var existentes = (await context.Categorias
                .Select(t => t.Nombre)
                .AsAsyncEnumerable()
                .ToListAsync(ct))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var (nombre, descripcion) in Datos.Where(d => !existentes.Contains(d.Nombre)))
            {
                var result = Categoria.Crear(nombre, descripcion);
                if (result.IsFailure)
                    throw new InvalidOperationException($"Seed inválido '{nombre}': {result.Error.Description}");

                context.Categorias.Add(result.Value);
            }

            await context.SaveChangesAsync(ct);
        }
    }
}
