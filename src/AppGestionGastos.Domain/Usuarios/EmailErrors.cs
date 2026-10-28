namespace AppGestionGastos.Domain.Usuarios;

public static class EmailErrors
{
    public static readonly Error Vacio = Error.Validation("Email.Vacio", "El email es obligatorio.");
    public static readonly Error Invalido = Error.Validation("Email.Invalido", "El formato del email no es válido.");
}
