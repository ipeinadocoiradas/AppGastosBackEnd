using AppGestionGastos.Domain.Cuentas.Events;

namespace AppGestionGastos.Domain.Cuentas;

public sealed class Banco : AuditableEntity
{
    private Banco() { }

    private Banco(Guid id, string nombre, string codigoInstitucional) : base(id)
    {
        Nombre = nombre;
        CodigoInstitucional = codigoInstitucional;
    }

    public string Nombre { get; private set; } = null!;
    public string CodigoInstitucional { get; private set; } = null!;

    public static Result<Banco> Crear(string nombre, string codigoInstitucional)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CuentaErrors.BancoNombreVacio;
        if (string.IsNullOrWhiteSpace(codigoInstitucional)) return CuentaErrors.BancoCodigoVacio;

        var banco = new Banco(Guid.CreateVersion7(), nombre.Trim(), codigoInstitucional.Trim());
        banco.Raise(new BancoCreadoEvent(banco.Id));

        return banco;
    }

    public Result Actualizar(string nombre, string codigoInstitucional)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CuentaErrors.BancoNombreVacio;
        if (string.IsNullOrWhiteSpace(codigoInstitucional)) return CuentaErrors.BancoCodigoVacio;

        Nombre = nombre.Trim();
        CodigoInstitucional = codigoInstitucional.Trim();
        Raise(new BancoActualizadoEvent(Id));

        return Result.Success();
    }

    /// <summary>Registra el evento de baja; el borrado físico lo hace el repositorio.</summary>
    public void Eliminar() => Raise(new BancoEliminadoEvent(Id));
}
