namespace AppGestionGastos.Domain.Usuarios;

public static class Rol
{
    public const string Admin = "Admin";
    public const string Usuario = "Usuario";
    public static readonly IReadOnlyCollection<string> Todos = [Admin, Usuario];
    public static bool EsValido(string rol) => Todos.Contains(rol);
}
