using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Movimientos.Events;
using Microsoft.Extensions.Logging;

namespace AppGestionGastos.Application.Features.Movimientos.EventHandlers;

/// <summary>
/// Actualiza el saldo de la cuenta afectada. No hace commit: el <see cref="IUnitOfWork"/> persiste
/// el movimiento y el nuevo saldo en la misma transacción.
/// </summary>
internal sealed class MovimientoRegistradoEventHandler(
    ICuentaRepository cuentas,
    ILogger<MovimientoRegistradoEventHandler> logger)
    : IDomainEventHandler<MovimientoRegistradoEvent>
{
    public async Task<Result> HandleAsync(
        MovimientoRegistradoEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var cuenta = await cuentas.GetByIdAsync(domainEvent.CuentaId, cancellationToken);
        if (cuenta is null) return CuentaErrors.NoEncontrada;

        var saldoAnterior = cuenta.SaldoActual;

        var resultado = cuenta.AplicarMovimiento(domainEvent.Monto, domainEvent.Naturaleza);
        if (resultado.IsFailure) return resultado;

        logger.LogInformation(
            "Saldo de la cuenta {CuentaId} actualizado por {Naturaleza} de {Monto}: {SaldoAnterior} -> {SaldoActual}",
            cuenta.Id,
            domainEvent.Naturaleza,
            domainEvent.Monto,
            saldoAnterior,
            cuenta.SaldoActual);

        return Result.Success();
    }
}
