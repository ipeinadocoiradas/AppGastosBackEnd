using System.ComponentModel.DataAnnotations;

namespace AppGestionGastos.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required] public string Issuer { get; init; } = string.Empty;
    [Required] public string Audience { get; init; } = string.Empty;

    /// <summary>Clave simétrica HS256: mínimo 32 caracteres. En producción, usar secretos/Key Vault.</summary>
    [Required, MinLength(32)] public string SecretKey { get; init; } = string.Empty;

    [Range(1, 1440)] public int ExpirationMinutes { get; init; } = 60;
}
