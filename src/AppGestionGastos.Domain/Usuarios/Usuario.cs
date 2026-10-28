using AppGestionGastos.Domain.Usuarios.Events;

namespace AppGestionGastos.Domain.Usuarios;

public sealed class Usuario : Entity
{
    private Usuario()
    {
        // Requerido por EF Core.
    }

    private Usuario(Guid id, string nombre, Email email, string passwordHash, IReadOnlyCollection<string> roles)
        : base(id)
    {
        Nombre = nombre;
        Email = email;
        PasswordHash = passwordHash;
        Roles = roles;
    }

    public string Nombre { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public IReadOnlyCollection<string> Roles { get; private set; } = [];

    public static Result<Usuario> Crear(
        string nombre,
        Email email,
        string passwordHash,
        IReadOnlyCollection<string> roles)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return UsuarioErrors.NombreVacio;
        if (string.IsNullOrWhiteSpace(passwordHash)) return UsuarioErrors.PasswordHashVacio;
        if (roles.Count == 0) return UsuarioErrors.SinRoles;

        var usuario = new Usuario(Guid.CreateVersion7(), nombre.Trim(), email, passwordHash, [.. roles.Distinct()]);
        usuario.Raise(new UsuarioCreadoEvent(usuario.Id, usuario.Nombre, usuario.Email.Value));

        return usuario;
    }

    public Result Actualizar(string nombre, Email email, IReadOnlyCollection<string> roles)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return UsuarioErrors.NombreVacio;
        if (roles.Count == 0) return UsuarioErrors.SinRoles;
        if (!roles.All(Rol.EsValido)) return UsuarioErrors.RolInvalido;

        Nombre = nombre.Trim();
        Email = email;
        Roles = [.. roles.Distinct()];

        return Result.Success();
    }
}
