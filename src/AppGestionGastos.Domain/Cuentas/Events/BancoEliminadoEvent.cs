namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record BancoEliminadoEvent(Guid BancoId) : DomainEvent;
}
