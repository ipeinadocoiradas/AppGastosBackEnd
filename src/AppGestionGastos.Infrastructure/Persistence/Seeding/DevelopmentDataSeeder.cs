using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace AppGestionGastos.Infrastructure.Persistence.Seeding
{   
        internal sealed class DevelopmentDataSeeder(
            AppDbContext context,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            ILogger<DevelopmentDataSeeder> logger)
        {
            public async Task SeedAsync(CancellationToken ct = default)
            {
                var password = configuration["Seeding:DefaultPassword"]
                    ?? throw new InvalidOperationException("Falta 'Seeding:DefaultPassword' en la configuración.");

                await SeedUsuarioAsync("Administrador", "admin@appgastos.local", password, [Rol.Admin, Rol.Usuario], ct);
                await SeedUsuarioAsync("Usuario Demo", "demo@appgastos.local", password, [Rol.Usuario], ct);

                await context.SaveChangesAsync(ct);
            }

            private async Task SeedUsuarioAsync(
                string nombre, string emailValue, string password, string[] roles, CancellationToken ct)
            {
                var email = Email.Create(emailValue).Value;
                
                if (await context.Usuarios.AnyAsync(u => u.Email == email, ct)) return;

                var result = Usuario.Crear(nombre, email, passwordHasher.Hash(password), roles);
                if (result.IsFailure)
                    throw new InvalidOperationException($"Seed de usuario inválido '{emailValue}': {result.Error.Description}");

                context.Usuarios.Add(result.Value);
                logger.LogInformation("Usuario de desarrollo creado: {Email}", emailValue);
            }
        }
    
}
