using AppGestionGastos.Domain.Movimientos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppGestionGastos.Infrastructure.Persistence.Configurations;

public sealed class TipoMovimientoConfiguration : IEntityTypeConfiguration<TipoMovimiento>
{
    private static readonly DateTimeOffset FechaSemilla = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public void Configure(EntityTypeBuilder<TipoMovimiento> builder)
    {
        builder.ToTable("TiposMovimiento");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Nombre).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Descripcion).HasMaxLength(250);
        builder.Property(t => t.Naturaleza).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.HasData(
        new
        {
            Id = TipoMovimiento.IngresoId,
            Nombre = "Ingreso",
            Descripcion = "Entrada de dinero a la cuenta",
            Naturaleza = NaturalezaMovimiento.Ingreso,
            CreadoEn = FechaSemilla
        },
        new
        {
            Id = TipoMovimiento.EgresoId,
            Nombre = "Egreso",
            Descripcion = "Salida de dinero de la cuenta",
            Naturaleza = NaturalezaMovimiento.Egreso,
            CreadoEn = FechaSemilla
        });       
    }
}
