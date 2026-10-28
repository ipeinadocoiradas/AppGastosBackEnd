using AppGestionGastos.Domain.Cuentas.Events;

namespace AppGestionGastos.Domain.Cuentas;

public sealed class TipoCuenta : AuditableEntity
{
    private TipoCuenta() { }

    private TipoCuenta(Guid id, string nombre, string? descripcion) : base(id)
    {
        Nombre = nombre;
        Descripcion = descripcion;
    }

    public string Nombre { get; private set; } = null!;
    public string? Descripcion { get; private set; }

    public static Result<TipoCuenta> Crear(string nombre, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CuentaErrors.TipoCuentaNombreVacio;

        var tipoCuenta = new TipoCuenta(Guid.CreateVersion7(), nombre.Trim(), descripcion?.Trim());
        tipoCuenta.Raise(new TipoCuentaCreadoEvent(tipoCuenta.Id));

        return tipoCuenta;
    }

    public Result Actualizar(string nombre, string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CuentaErrors.TipoCuentaNombreVacio;

        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim();
        Raise(new TipoCuentaActualizadoEvent(Id));

        return Result.Success();
    }

    public void Eliminar() => Raise(new TipoCuentaEliminadoEvent(Id));
}
