namespace AppGestionGastos.Application.Features.Auth.Login;

public sealed record LoginResponse(string AccessToken, DateTimeOffset ExpiresAt);
