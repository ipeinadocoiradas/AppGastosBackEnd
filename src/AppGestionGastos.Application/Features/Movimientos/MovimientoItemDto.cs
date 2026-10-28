using AppGestionGastos.Domain.Movimientos;


namespace AppGestionGastos.Application.Features.Movimientos
{
    public sealed record MovimientoItemDto(
     Guid Id,
     Guid CuentaId,
     string NumeroCuenta,
     Guid CategoriaId,
     string Categoria,
     Guid TipoMovimientoId,
     string TipoMovimiento,
     NaturalezaMovimiento Naturaleza,
     decimal Monto,
     DateTimeOffset FechaHora,
     string? Descripcion);
}
