using AppGestionGastos.Domain.Categorias;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Movimientos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppGestionGastos.Infrastructure.Persistence.Configurations;

public sealed class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
{
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("Movimientos");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Monto).HasPrecision(18, 2);
        builder.Property(m => m.FechaHora).IsRequired();
        builder.Property(m => m.Descripcion).HasMaxLength(Movimiento.DescripcionMaxLength);
        builder.HasIndex(m => new { m.CuentaId, m.FechaHora });

        builder.HasOne<Cuenta>().WithMany().HasForeignKey(m => m.CuentaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Categoria>().WithMany().HasForeignKey(m => m.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<TipoMovimiento>().WithMany().HasForeignKey(m => m.TipoMovimientoId).OnDelete(DeleteBehavior.Restrict);
    }
}
