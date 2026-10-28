namespace AppGestionGastos.Domain.Usuarios;

public static class UsuarioErrors
{
    public static readonly Error NombreVacio = Error.Validation("Usuario.NombreVacio", "El nombre es obligatorio.");
    public static readonly Error PasswordHashVacio = Error.Validation("Usuario.PasswordHashVacio", "El hash de contraseña es obligatorio.");
    public static readonly Error SinRoles = Error.Validation("Usuario.SinRoles", "El usuario debe tener al menos un rol.");
    public static readonly Error EmailYaRegistrado = Error.Conflict("Usuario.EmailYaRegistrado", "Ya existe un usuario con ese email.");
    public static readonly Error CredencialesInvalidas = Error.Unauthorized("Usuario.CredencialesInvalidas", "Email o contraseña incorrectos.");
    public static readonly Error NoAutenticado = Error.Unauthorized("Usuario.NoAutenticado", "No se pudo identificar al usuario autenticado.");
    public static readonly Error NoEncontrado = Error.NotFound("Usuario.NoEncontrado", "El usuario no existe.");
    public static readonly Error RolInvalido = Error.Validation("Usuario.RolInvalido", "Alguno de los roles indicados no es válido.");
    public static readonly Error NoPuedeQuitarseAdmin = Error.Conflict("Usuario.NoPuedeQuitarseAdmin", "No puedes quitarte a ti mismo el rol de administrador.");
    public static readonly Error NoPuedeEliminarseASiMismo = Error.Conflict("Usuario.NoPuedeEliminarseASiMismo", "No puedes eliminar tu propio usuario.");
    public static readonly Error TieneCuentas = Error.Conflict("Usuario.TieneCuentas", "No se puede eliminar el usuario porque tiene cuentas asociadas.");
}
