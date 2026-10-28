using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppGestionGastos.Infrastructure.Persistence.Configurations;

public sealed class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("Cuentas");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.NumeroCuenta).HasMaxLength(34).IsRequired();
        builder.Property(c => c.SaldoActual).HasPrecision(18, 2);
        builder.HasIndex(c => new { c.BancoId, c.NumeroCuenta }).IsUnique();
        builder.HasIndex(c => c.UsuarioId);

        // Concurrencia optimista: evita perder actualizaciones de saldo concurrentes.
        builder.Property<byte[]>("RowVersion").IsRowVersion();

        builder.HasOne<Usuario>().WithMany().HasForeignKey(c => c.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Banco>().WithMany().HasForeignKey(c => c.BancoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<TipoCuenta>().WithMany().HasForeignKey(c => c.TipoCuentaId).OnDelete(DeleteBehavior.Restrict);
    }
}
