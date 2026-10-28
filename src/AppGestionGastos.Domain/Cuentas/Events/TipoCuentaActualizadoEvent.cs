namespace AppGestionGastos.Domain.Cuentas.Events
{
    public sealed record TipoCuentaActualizadoEvent(Guid TipoCuentaId) : DomainEvent;
}
