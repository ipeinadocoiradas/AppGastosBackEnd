namespace AppGestionGastos.Domain.Movimientos;

public sealed class TipoMovimiento : AuditableEntity
{
    // Identificadores fijos de los datos semilla (catálogo cerrado).
    public static readonly Guid IngresoId = Guid.Parse("00000000-0000-7000-8000-000000000001");
    public static readonly Guid EgresoId = Guid.Parse("00000000-0000-7000-8000-000000000002");

    private TipoMovimiento() { }

    private TipoMovimiento(Guid id, string nombre, string? descripcion, NaturalezaMovimiento naturaleza) : base(id)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Naturaleza = naturaleza;
    }

    public string Nombre { get; private set; } = null!;
    public string? Descripcion { get; private set; }
    public NaturalezaMovimiento Naturaleza { get; private set; }

    public static Result<TipoMovimiento> Crear(string nombre, NaturalezaMovimiento naturaleza, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return TipoMovimientoErrors.NombreVacio;

        return new TipoMovimiento(Guid.CreateVersion7(), nombre.Trim(), descripcion?.Trim(), naturaleza);
    }
}
