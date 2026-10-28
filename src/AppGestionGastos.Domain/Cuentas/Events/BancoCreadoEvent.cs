namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record BancoCreadoEvent(Guid BancoId) : DomainEvent;
}
