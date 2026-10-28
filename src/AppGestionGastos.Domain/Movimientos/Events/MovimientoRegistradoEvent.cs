namespace AppGestionGastos.Domain.Movimientos.Events;

public sealed record MovimientoRegistradoEvent(
    Guid MovimientoId,
    Guid CuentaId,
    decimal Monto,
    NaturalezaMovimiento Naturaleza) : DomainEvent;
