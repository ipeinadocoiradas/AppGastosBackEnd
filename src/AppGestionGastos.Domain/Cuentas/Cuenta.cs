using AppGestionGastos.Domain.Movimientos;

namespace AppGestionGastos.Domain.Cuentas;

public sealed class Cuenta : AuditableEntity
{
    private Cuenta() { }

    private Cuenta(Guid id, string numeroCuenta, decimal saldoActual, Guid usuarioId, Guid bancoId, Guid tipoCuentaId)
        : base(id)
    {
        NumeroCuenta = numeroCuenta;
        SaldoActual = saldoActual;
        UsuarioId = usuarioId;
        BancoId = bancoId;
        TipoCuentaId = tipoCuentaId;
    }

    public string NumeroCuenta { get; private set; } = null!;
    public decimal SaldoActual { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid BancoId { get; private set; }
    public Guid TipoCuentaId { get; private set; }

    public static Result<Cuenta> Crear(
        string numeroCuenta,
        Guid usuarioId,
        Guid bancoId,
        Guid tipoCuentaId,
        decimal saldoInicial = 0m)
    {
        if (string.IsNullOrWhiteSpace(numeroCuenta)) return CuentaErrors.NumeroVacio;

        return new Cuenta(Guid.CreateVersion7(), numeroCuenta.Trim(), saldoInicial, usuarioId, bancoId, tipoCuentaId);
    }

    /// <summary>Suma el monto si el movimiento es un ingreso y lo resta si es un egreso.</summary>
    public Result AplicarMovimiento(decimal monto, NaturalezaMovimiento naturaleza)
    {
        if (monto <= 0) return CuentaErrors.MontoInvalido;

        SaldoActual = naturaleza == NaturalezaMovimiento.Ingreso
            ? SaldoActual + monto
            : SaldoActual - monto;

        return Result.Success();
    }

    /// <summary>El saldo no se puede modificar desde aquí: solo cambia con movimientos.</summary>
    public Result Actualizar(string numeroCuenta, Guid bancoId, Guid tipoCuentaId)
    {
        if (string.IsNullOrWhiteSpace(numeroCuenta)) return CuentaErrors.NumeroVacio;

        NumeroCuenta = numeroCuenta.Trim();
        BancoId = bancoId;
        TipoCuentaId = tipoCuentaId;

        return Result.Success();
    }
}
