using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Application.Abstractions.Caching;
using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Features.Bancos;
using AppGestionGastos.Application.Features.Categorias;
using AppGestionGastos.Application.Features.Cuentas;
using AppGestionGastos.Application.Features.Movimientos;
using AppGestionGastos.Application.Features.TiposCuenta;
using AppGestionGastos.Application.Features.TiposMovimiento;
using AppGestionGastos.Application.Features.Usuarios;
using AppGestionGastos.Domain.Categorias;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Movimientos;
using AppGestionGastos.Domain.Usuarios;
using AppGestionGastos.Infrastructure.Authentication;
using AppGestionGastos.Infrastructure.Caching;
using AppGestionGastos.Infrastructure.Logging;
using AppGestionGastos.Infrastructure.Logging.LogRetention;
using AppGestionGastos.Infrastructure.Messaging;
using AppGestionGastos.Infrastructure.Persistence;
using AppGestionGastos.Infrastructure.Persistence.Auditing;
using AppGestionGastos.Infrastructure.Persistence.CachingRepositories;
using AppGestionGastos.Infrastructure.Persistence.Diagnostics;
using AppGestionGastos.Infrastructure.Persistence.ReadRepositories;
using AppGestionGastos.Infrastructure.Persistence.Repositories;
using AppGestionGastos.Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppGestionGastos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'DefaultConnection'.");

        services.AddSingleton(TimeProvider.System);

        // Opciones: se validan al arrancar
        services.AddOptions<DbCommandTimingOptions>()
            .Bind(configuration.GetSection(DbCommandTimingOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(
                o => o.ErrorThresholdMs >= o.WarningThresholdMs,
                "DbCommandTiming: ErrorThresholdMs debe ser mayor o igual que WarningThresholdMs.")
            .ValidateOnStart();

        // Opciones: se validan al arrancar
        services.AddOptions<LogSinksOptions>()
            .Bind(configuration.GetSection(LogSinksOptions.SectionName))
                .Validate(o => o.HasValidLogTable,
                "LogSinks:Database: SchemaName y TableName solo admiten letras, números y '_'.")
                .ValidateOnStart();

        // Opciones: se validan al arrancar
        services.AddOptions<LogRetentionOptions>()
            .Bind(configuration.GetSection(LogRetentionOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Eliminar logs
        services.AddHostedService<LogRetentionService>();

        // Caché híbrida (L1 en memoria)
        services.AddHybridCache();

        // Interceptores
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddSingleton<DbCommandTimingInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) => options
            .UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure().CommandTimeout(30))
            .AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(), 
                serviceProvider.GetRequiredService<DbCommandTimingInterceptor>()));

        // Unidad de Trabajo
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Catálogo de caché
        services.AddSingleton<CatalogCache>();
        services.AddSingleton<ICatalogCacheInvalidator, HybridCatalogCacheInvalidator>();

        // Repositorios
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICuentaRepository, CuentaRepository>();
        services.AddScoped<ITipoMovimientoRepository, TipoMovimientoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IMovimientoRepository, MovimientoRepository>();
        services.AddScoped<IBancoRepository, BancoRepository>();
        services.AddScoped<ITipoCuentaRepository, TipoCuentaRepository>();

        // Repositorios Lectura        
        services.AddScoped<BancoReadRepository>();          //Con caché
        services.AddScoped<TipoCuentaReadRepository>();     //Con caché
        services.AddScoped<CategoriaReadRepository>();      //Con caché
        services.AddScoped<TipoMovimientoReadRepository>();      //Con caché

        services.AddScoped<IBancoReadRepository, CachedBancoReadRepository>();
        services.AddScoped<ITipoCuentaReadRepository, CachedTipoCuentaReadRepository>();
        services.AddScoped<ICategoriaReadRepository, CachedCategoriaReadRepository>();
        services.AddScoped<ITipoMovimientoReadRepository, CachedTipoMovimientoReadRepository>();
        services.AddScoped<ICuentaReadRepository, CuentaReadRepository>();
        services.AddScoped<IUsuarioReadRepository, UsuarioReadRepository>();
        services.AddScoped<IMovimientoReadRepository, MovimientoReadRepository>();        

        // Eventos de dominio
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IPostCommitEventDispatcher, PostCommitEventDispatcher>();

        // JWT
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

        //Seed
        services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure().CommandTimeout(30))
            .UseSeeding((ctx, _) => DatabaseSeeder.RunAsync(ctx, default).GetAwaiter().GetResult())
            .UseAsyncSeeding((ctx, _, ct) => DatabaseSeeder.RunAsync(ctx, ct)));

        return services;
    }
}
