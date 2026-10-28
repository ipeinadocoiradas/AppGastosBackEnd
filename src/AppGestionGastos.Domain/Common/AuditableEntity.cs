

namespace AppGestionGastos.Domain.Common
{
    /// <summary>
    /// Entidad con datos de auditoría. Los rellena la infraestructura al guardar:
    /// el dominio los expone en solo lectura y no puede alterarlos.
    /// </summary>
    public abstract class AuditableEntity : Entity
    {
        protected AuditableEntity()
        {
            // Requerido por EF Core.
        }

        protected AuditableEntity(Guid id) : base(id) { }

        public DateTimeOffset CreadoEn { get; private set; }
        public Guid? CreadoPor { get; private set; }
        public DateTimeOffset? ModificadoEn { get; private set; }
        public Guid? ModificadoPor { get; private set; }
    }
}
