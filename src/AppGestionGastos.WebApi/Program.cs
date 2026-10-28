using AppGestionGastos.Application;
using AppGestionGastos.Domain.Usuarios;
using AppGestionGastos.Infrastructure;
using AppGestionGastos.Infrastructure.Logging;
using AppGestionGastos.Infrastructure.Persistence;
using AppGestionGastos.Infrastructure.Persistence.Seeding;
using AppGestionGastos.WebApi;
using AppGestionGastos.WebApi.Authorization;
using AppGestionGastos.WebApi.Configuration;
using AppGestionGastos.WebApi.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;

// Logger de arranque: captura errores de configuración antes de que Serilog esté totalmente configurado.
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilogLogging();

    const string MisOrigenesPermitidos = "_misOrigenesPermitidos";

    // 2. Configurar el servicio de CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MisOrigenesPermitidos,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200") // El origen de tu app de Angular/React/etc.
                      .AllowAnyHeader()                     // Permite cualquier cabecera (Authorization, Content-Type, etc.)
                      .AllowAnyMethod();                    // Permite cualquier método HTTP (GET, POST, PUT, DELETE)
            });
    });

    builder.Services.AddControllers();

    // Mensajes de FluentValidation en español.
    ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("es");

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy(Policies.AdminOnly, policy => policy
            .RequireAuthenticatedUser()
            .RequireRole(Rol.Admin));

    // Configuración OpenApi personalizada
    builder.Services.AddCustomOpenApi();

  
    var app = builder.Build();  

    // Orden importante: el middleware de tiempos envuelve al de excepciones para registrar el status final (p. ej. 500).
    app.UseMiddleware<RequestTimingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseStatusCodePages();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();       
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("Web API de Gestión de Gastos");            
            options.ShowDeveloperTools = DeveloperToolsVisibility.Never;            
        }); 

        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Aplica migraciones pendientes y ejecuta UseAsyncSeeding
        await db.Database.MigrateAsync();  

        // Datos de prueba para desarrollo
        await app.Services.SeedDevelopmentDataAsync();
    }
    app.UseHttpsRedirection();
    app.UseCors(MisOrigenesPermitidos);
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "La aplicación terminó inesperadamente durante el arranque");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;
