namespace AppGestionGastos.Domain.Categorias.Events
{
    public sealed record CategoriaCreadaEvent(Guid CategoriaId) : DomainEvent;
}
