using AppGestionGastos.Domain.Movimientos;

namespace AppGestionGastos.Application.Features.TiposMovimiento
{
    public sealed record TipoMovimientoDto(
     Guid Id,
     string Nombre,
     string? Descripcion,
     NaturalezaMovimiento Naturaleza);
}
