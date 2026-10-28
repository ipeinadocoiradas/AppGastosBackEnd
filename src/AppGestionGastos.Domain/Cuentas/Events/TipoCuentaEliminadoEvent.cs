namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record TipoCuentaEliminadoEvent(Guid TipoCuentaId) : DomainEvent;
}
