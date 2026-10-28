using AppGestionGastos.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppGestionGastos.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();

        builder.Property(u => u.Nombre).HasMaxLength(100).IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(email => email.Value, value => Email.Create(value).Value)
            .HasMaxLength(Email.MaxLength)
            .IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();

        builder.Property(u => u.Roles)
            .HasConversion(
                roles => string.Join(',', roles),
                value => (IReadOnlyCollection<string>)value.Split(',', StringSplitOptions.RemoveEmptyEntries),
                new ValueComparer<IReadOnlyCollection<string>>(
                    (a, b) => a!.SequenceEqual(b!),
                    roles => roles.Aggregate(0, (hash, rol) => HashCode.Combine(hash, rol.GetHashCode())),
                    roles => roles.ToArray()))
            .HasMaxLength(200)
            .IsRequired();
    }
}
