using AppGestionGastos.Domain.Categorias;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Movimientos;
using AppGestionGastos.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Banco> Bancos => Set<Banco>();
    public DbSet<TipoCuenta> TiposCuenta => Set<TipoCuenta>();
    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<TipoMovimiento> TiposMovimiento => Set<TipoMovimiento>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Las columnas de auditoría se colocan al final de cada tabla.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(AuditableEntity).IsAssignableFrom(t.ClrType)))
        {
            var entity = modelBuilder.Entity(entityType.ClrType);
            entity.Property(nameof(AuditableEntity.CreadoEn)).HasColumnOrder(1000);
            entity.Property(nameof(AuditableEntity.CreadoPor)).HasColumnOrder(1001);
            entity.Property(nameof(AuditableEntity.ModificadoEn)).HasColumnOrder(1002);
            entity.Property(nameof(AuditableEntity.ModificadoPor)).HasColumnOrder(1003);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
}
