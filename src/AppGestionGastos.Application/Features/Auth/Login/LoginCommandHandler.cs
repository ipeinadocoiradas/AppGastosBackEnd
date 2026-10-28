using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler(
    IUsuarioRepository usuarios,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator tokenGenerator)
    : ICommandHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        // Mismo error para email inexistente, mal formado o contraseña incorrecta (evita enumerar usuarios).
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure) return UsuarioErrors.CredencialesInvalidas;

        var usuario = await usuarios.GetByEmailAsync(emailResult.Value, cancellationToken);

        if (usuario is null || !passwordHasher.Verify(command.Password, usuario.PasswordHash))
        {
            return UsuarioErrors.CredencialesInvalidas;
        }

        var token = tokenGenerator.Generate(usuario);

        return new LoginResponse(token.Value, token.ExpiresAt);
    }
}
