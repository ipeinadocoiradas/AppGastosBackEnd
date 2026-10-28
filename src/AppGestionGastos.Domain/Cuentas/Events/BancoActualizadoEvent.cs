namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record BancoActualizadoEvent(Guid BancoId) : DomainEvent;
}
