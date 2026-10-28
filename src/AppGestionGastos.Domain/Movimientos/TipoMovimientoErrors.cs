namespace AppGestionGastos.Domain.Movimientos;

public static class TipoMovimientoErrors
{
    public static readonly Error NoEncontrado = Error.NotFound("TipoMovimiento.NoEncontrado", "El tipo de movimiento no existe.");
    public static readonly Error NombreVacio = Error.Validation("TipoMovimiento.NombreVacio", "El nombre del tipo de movimiento es obligatorio.");
}
