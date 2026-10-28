using AppGestionGastos.Domain.Categorias.Events;

namespace AppGestionGastos.Domain.Categorias;

public sealed class Categoria : AuditableEntity
{
    private Categoria() { }

    private Categoria(Guid id, string nombre, string? descripcion) : base(id)
    {
        Nombre = nombre;
        Descripcion = descripcion;
    }

    public string Nombre { get; private set; } = null!;
    public string? Descripcion { get; private set; }

    public static Result<Categoria> Crear(string nombre, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CategoriaErrors.NombreVacio;

        var categoria = new Categoria(Guid.CreateVersion7(), nombre.Trim(), descripcion?.Trim());
        categoria.Raise(new CategoriaCreadaEvent(categoria.Id));

        return categoria;
    }

    public Result Actualizar(string nombre, string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return CategoriaErrors.NombreVacio;

        Nombre = nombre.Trim();
        Descripcion = descripcion?.Trim();
        Raise(new CategoriaActualizadaEvent(Id));

        return Result.Success();
    }

    public void Eliminar() => Raise(new CategoriaEliminadaEvent(Id));
}
