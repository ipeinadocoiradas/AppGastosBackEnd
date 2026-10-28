using System.Net.Mail;

namespace AppGestionGastos.Domain.Usuarios;

/// <summary>Objeto de valor: email normalizado (trim + minúsculas).</summary>
public sealed record Email
{
    public const int MaxLength = 254;

    private Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return EmailErrors.Vacio;
        }

        var normalizado = email.Trim().ToLowerInvariant();

        if (normalizado.Length > MaxLength
            || !MailAddress.TryCreate(normalizado, out var parsed)
            || parsed.Address != normalizado)
        {
            return EmailErrors.Invalido;
        }

        return new Email(normalizado);
    }

    public override string ToString() => Value;
}
