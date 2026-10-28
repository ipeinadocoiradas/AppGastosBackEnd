using AppGestionGastos.Application.Abstractions.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace AppGestionGastos.Infrastructure.Persistence.Auditing
{
    /// <summary>Rellena los campos de auditoría de toda entidad auditable al guardar.</summary>
    internal sealed class AuditSaveChangesInterceptor(ICurrentUser currentUser, TimeProvider timeProvider)
        : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAudit(DbContext? context)
        {
            if (context is null) return;

            var now = timeProvider.GetUtcNow();
            var userId = currentUser.UserId;   // null fuera de una petición (seeders) o si es anónima

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(e => e.CreadoEn).CurrentValue = now;
                        entry.Property(e => e.CreadoPor).CurrentValue = userId;
                        break;

                    case EntityState.Modified:
                        // Los datos de creación nunca se sobrescriben.
                        entry.Property(e => e.CreadoEn).IsModified = false;
                        entry.Property(e => e.CreadoPor).IsModified = false;
                        entry.Property(e => e.ModificadoEn).CurrentValue = now;
                        entry.Property(e => e.ModificadoPor).CurrentValue = userId;
                        break;
                }
            }
        }
    }
}
