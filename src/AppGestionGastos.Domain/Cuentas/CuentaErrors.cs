namespace AppGestionGastos.Domain.Cuentas;

public static class CuentaErrors
{
    public static readonly Error NoEncontrada = Error.NotFound("Cuenta.NoEncontrada", "La cuenta no existe.");
    public static readonly Error NumeroVacio = Error.Validation("Cuenta.NumeroVacio", "El número de cuenta es obligatorio.");
    public static readonly Error MontoInvalido = Error.Validation("Cuenta.MontoInvalido", "El monto del movimiento debe ser mayor que cero.");
    public static readonly Error BancoNombreVacio = Error.Validation("Banco.NombreVacio", "El nombre del banco es obligatorio.");
    public static readonly Error BancoCodigoVacio = Error.Validation("Banco.CodigoVacio", "El código institucional es obligatorio.");
    public static readonly Error TipoCuentaNombreVacio = Error.Validation("TipoCuenta.NombreVacio", "El nombre del tipo de cuenta es obligatorio.");
    public static readonly Error BancoCodigoYaRegistrado = Error.Conflict("Banco.CodigoYaRegistrado", "Ya existe un banco con ese código institucional.");
    public static readonly Error TipoCuentaNombreYaRegistrado = Error.Conflict("TipoCuenta.NombreYaRegistrado", "Ya existe un tipo de cuenta con ese nombre.");
    public static readonly Error BancoNoEncontrado = Error.NotFound("Cuenta.BancoNoEncontrado", "El banco indicado no existe.");
    public static readonly Error TipoCuentaNoEncontrado = Error.NotFound("Cuenta.TipoCuentaNoEncontrado", "El tipo de cuenta indicado no existe.");
    public static readonly Error NumeroYaRegistrado = Error.Conflict("Cuenta.NumeroYaRegistrado", "Ya existe una cuenta con ese número en ese banco.");
    public static readonly Error BancoEnUso = Error.Conflict("Banco.EnUso", "No se puede eliminar el banco porque tiene cuentas asociadas.");
    public static readonly Error TipoCuentaEnUso = Error.Conflict("TipoCuenta.EnUso", "No se puede eliminar el tipo de cuenta porque tiene cuentas asociadas.");
}
