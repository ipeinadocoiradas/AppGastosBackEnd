namespace AppGestionGastos.Domain.Movimientos;

/// <summary>Define si un tipo de movimiento suma o resta saldo (evita comparar nombres por texto).</summary>
public enum NaturalezaMovimiento
{
    Ingreso = 1,
    Egreso = 2
}
