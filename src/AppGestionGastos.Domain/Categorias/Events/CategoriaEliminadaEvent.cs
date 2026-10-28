namespace AppGestionGastos.Domain.Categorias.Events
{
    public sealed record CategoriaEliminadaEvent(Guid CategoriaId) : DomainEvent;
}
