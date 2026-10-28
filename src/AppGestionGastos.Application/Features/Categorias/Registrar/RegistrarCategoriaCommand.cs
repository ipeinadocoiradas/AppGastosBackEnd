namespace AppGestionGastos.Application.Features.Categorias.Crear;

public sealed record RegistrarCategoriaCommand(string Nombre, string? Descripcion) : ICommand<Result<CategoriaDto>>;