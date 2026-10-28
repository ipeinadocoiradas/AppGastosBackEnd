namespace AppGestionGastos.Domain.Categorias;

public static class CategoriaErrors
{
    public static readonly Error NoEncontrada = Error.NotFound("Categoria.NoEncontrada", "La categoría no existe.");
    public static readonly Error NombreVacio = Error.Validation("Categoria.NombreVacio", "El nombre de la categoría es obligatorio.");
    public static readonly Error NombreYaRegistrado = Error.Conflict("Categoria.NombreYaRegistrado", "Ya existe una categoría con ese nombre.");
    public static readonly Error EnUso = Error.Conflict("Categoria.EnUso", "No se puede eliminar la categoría porque tiene movimientos asociados.");
}
