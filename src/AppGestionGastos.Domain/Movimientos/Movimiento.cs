using AppGestionGastos.Domain.Movimientos.Events;

namespace AppGestionGastos.Domain.Movimientos;

public sealed class Movimiento : AuditableEntity
{
    public const int DescripcionMaxLength = 250;

    private Movimiento() { }

    private Movimiento(
        Guid id,
        decimal monto,
        DateTimeOffset fechaHora,
        string? descripcion,
        Guid cuentaId,
        Guid categoriaId,
        Guid tipoMovimientoId) : base(id)
    {
        Monto = monto;
        FechaHora = fechaHora;
        Descripcion = descripcion;
        CuentaId = cuentaId;
        CategoriaId = categoriaId;
        TipoMovimientoId = tipoMovimientoId;
    }

    public decimal Monto { get; private set; }
    public DateTimeOffset FechaHora { get; private set; }
    public string? Descripcion { get; private set; }
    public Guid CuentaId { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Guid TipoMovimientoId { get; private set; }

    public static Result<Movimiento> Registrar(
        Guid cuentaId,
        Guid categoriaId,
        TipoMovimiento tipoMovimiento,
        decimal monto,
        string? descripcion,
        DateTimeOffset fechaHora)
    {
        if (monto <= 0) return MovimientoErrors.MontoInvalido;
        if (descripcion is { Length: > DescripcionMaxLength }) return MovimientoErrors.DescripcionDemasiadoLarga;

        var movimiento = new Movimiento(
            Guid.CreateVersion7(),
            monto,
            fechaHora,
            descripcion?.Trim(),
            cuentaId,
            categoriaId,
            tipoMovimiento.Id);

        movimiento.Raise(new MovimientoRegistradoEvent(movimiento.Id, cuentaId, monto, tipoMovimiento.Naturaleza));

        return movimiento;
    }
}
