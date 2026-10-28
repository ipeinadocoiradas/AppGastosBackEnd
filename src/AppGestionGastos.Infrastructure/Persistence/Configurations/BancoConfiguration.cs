using AppGestionGastos.Domain.Cuentas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppGestionGastos.Infrastructure.Persistence.Configurations;

public sealed class BancoConfiguration : IEntityTypeConfiguration<Banco>
{
    public void Configure(EntityTypeBuilder<Banco> builder)
    {
        builder.ToTable("Bancos");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property(b => b.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(b => b.CodigoInstitucional).HasMaxLength(20).IsRequired();
        builder.HasIndex(b => b.CodigoInstitucional).IsUnique();
    }
}
