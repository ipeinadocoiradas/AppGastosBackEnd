using Microsoft.OpenApi;

namespace AppGestionGastos.WebApi.Configuration
{
    public static class OpenApiConfiguration
    {
        public static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                // 1. Configuración de la metadata general (Título, Descripción, etc.)
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "Mi API en .NET 10 C# 🤖",
                        Version = "v1.0.0",
                        Description = """
                    Bienvenido a la documentación de la API. Aquí puedes gestionar los controladores e integraciones.
                    
                    ### Características:
                    * CQRS. DDD. Repository y Unit Of Work. JWT. Patrón Result.          
                    * Eventos de Dominio: OnSaveChanges y Post-OnSaveChanges.
                    * Scalar. Fluent Validation. Serilog.
                    * Middleware: Rendimiento en Peticiones y control de Excepciones.
                    * Interceptores: Auditoría y Rendimiento en Consultas SQL.
                    * Caché: Hybrid Caché
                    * BackgroundService: Eliminación de logs.
                    
                    Usa el menú lateral para explorar y probar los endpoints.
                    """,
                        Contact = new OpenApiContact
                        {
                            Name = "Desarrollo y Arquitectura",
                            Email = "desarrollo@tuempresa.com"
                        }
                    };
                    return Task.CompletedTask;
                });

                // 2. Configuración y lectura del archivo XML de comentarios
                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

                if (File.Exists(xmlPath))
                {
                    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    {
                        // Tu lógica para procesar los comentarios XML de los controladores
                        return Task.CompletedTask;
                    });
                }
            });

            return services;
        }
    }
}
