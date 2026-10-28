namespace AppGestionGastos.Domain.Categorias.Events
{
    public sealed record CategoriaActualizadaEvent(Guid CategoriaId) : DomainEvent;
}
