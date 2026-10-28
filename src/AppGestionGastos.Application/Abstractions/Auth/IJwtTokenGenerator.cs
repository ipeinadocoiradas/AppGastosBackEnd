using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Abstractions.Auth;

public sealed record AccessToken(string Value, DateTimeOffset ExpiresAt);

public interface IJwtTokenGenerator
{
    AccessToken Generate(Usuario usuario);
}
