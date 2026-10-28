using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Categorias;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Movimientos;
using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Features.Movimientos.Registrar;

internal sealed class RegistrarMovimientoCommandHandler(
    ICuentaRepository cuentas,
    ITipoMovimientoRepository tiposMovimiento,
    ICategoriaRepository categorias,
    IMovimientoRepository movimientos,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<RegistrarMovimientoCommand, Result<MovimientoDto>>
{
    public async Task<Result<MovimientoDto>> HandleAsync(
        RegistrarMovimientoCommand command,
        CancellationToken cancellationToken = default)
    {
        if (currentUser.UserId is not { } usuarioId) return UsuarioErrors.NoAutenticado;

        var cuenta = await cuentas.GetByIdAsync(command.CuentaId, cancellationToken);

        // Una cuenta ajena se responde como "no encontrada" para no revelar su existencia.
        if (cuenta is null || cuenta.UsuarioId != usuarioId) return CuentaErrors.NoEncontrada;

        var tipoMovimiento = await tiposMovimiento.GetByIdAsync(command.TipoMovimientoId, cancellationToken);
        if (tipoMovimiento is null) return TipoMovimientoErrors.NoEncontrado;

        if (!await categorias.ExisteAsync(command.CategoriaId, cancellationToken))
        {
            return CategoriaErrors.NoEncontrada;
        }

        var movimientoResult = Movimiento.Registrar(
            cuenta.Id,
            command.CategoriaId,
            tipoMovimiento,
            command.Monto,
            command.Descripcion,
            command.FechaHora ?? timeProvider.GetUtcNow());

        if (movimientoResult.IsFailure) return movimientoResult.Error;

        var movimiento = movimientoResult.Value;
        movimientos.Add(movimiento);

        // Despacha MovimientoRegistradoEvent (actualiza el saldo) y persiste todo atómicamente.
        var commit = await unitOfWork.CommitAsync(cancellationToken);
        if (commit.IsFailure) return commit.Error;

        return new MovimientoDto(
            movimiento.Id,
            movimiento.CuentaId,
            movimiento.CategoriaId,
            movimiento.TipoMovimientoId,
            movimiento.Monto,
            movimiento.FechaHora,
            movimiento.Descripcion,
            cuenta.SaldoActual);
    }
}
