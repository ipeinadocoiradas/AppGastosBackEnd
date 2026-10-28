namespace AppGestionGastos.Application.Features.Usuarios.Registrar;

public sealed record RegistrarUsuarioCommand(string Nombre, string Email, string Password)
    : ICommand<Result<UsuarioDto>>;
