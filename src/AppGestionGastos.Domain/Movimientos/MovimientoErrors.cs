namespace AppGestionGastos.Domain.Movimientos;

public static class MovimientoErrors
{
    public static readonly Error MontoInvalido = Error.Validation("Movimiento.MontoInvalido", "El monto debe ser estrictamente mayor que cero.");
    public static readonly Error DescripcionDemasiadoLarga = Error.Validation(
        "Movimiento.DescripcionDemasiadoLarga",
        $"La descripción no puede superar {Movimiento.DescripcionMaxLength} caracteres.");
}
