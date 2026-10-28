namespace AppGestionGastos.Application.Features.Movimientos;

public sealed record MovimientoDto(
    Guid Id,
    Guid CuentaId,
    Guid CategoriaId,
    Guid TipoMovimientoId,
    decimal Monto,
    DateTimeOffset FechaHora,
    string? Descripcion,
    decimal SaldoActualCuenta);
