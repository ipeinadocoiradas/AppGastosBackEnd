namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record TipoCuentaCreadoEvent(Guid TipoCuentaId) : DomainEvent;
}
