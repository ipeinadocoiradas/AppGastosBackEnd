using System.ComponentModel.DataAnnotations.Schema;

namespace AppGestionGastos.Domain.Common;

public abstract class Entity
{
    private readonly List<IDomainEvent> _events = [];

    protected Entity()
    {
        // Requerido por EF Core.
    }

    protected Entity(Guid id) => Id = id;

    public Guid Id { get; private set; }

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    public void ClearDomainEvents() => _events.Clear();

    protected void Raise(IDomainEvent domainEvent) => _events.Add(domainEvent);
}
