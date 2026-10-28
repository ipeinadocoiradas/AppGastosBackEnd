namespace AppGestionGastos.Application.Features.Movimientos.Registrar;

/// <param name="FechaHora">Si es null se usa la fecha/hora actual (UTC).</param>
public sealed record RegistrarMovimientoCommand(
    Guid CuentaId,
    Guid CategoriaId,
    Guid TipoMovimientoId,
    decimal Monto,
    string? Descripcion,
    DateTimeOffset? FechaHora = null) : ICommand<Result<MovimientoDto>>;
