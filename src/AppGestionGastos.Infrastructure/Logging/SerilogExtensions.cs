using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace AppGestionGastos.Infrastructure.Logging;

public static class SerilogExtensions
{
    private const string ConsoleTemplate =
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

    /// <summary>
    /// Configura Serilog: niveles desde la sección "Serilog" y destinos desde "LogSinks"
    /// (consola, ficheros y/o base de datos). Cambiar los destinos requiere reiniciar.
    /// </summary>
    public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder) =>
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            var sinks = context.Configuration
                .GetSection(LogSinksOptions.SectionName)
                .Get<LogSinksOptions>() ?? new LogSinksOptions();
            sinks.Validate();

            configuration
                .ReadFrom.Configuration(context.Configuration)   // MinimumLevel y Override
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "AppGestionGastos");

            if (sinks.Console.Enabled)
            {
                configuration.WriteTo.Console(
                    outputTemplate: ConsoleTemplate,
                    restrictedToMinimumLevel: sinks.Console.MinimumLevel);
            }

            if (sinks.File.Enabled)
            {
                configuration.WriteTo.File(
                    path: sinks.File.Path,
                    rollingInterval: sinks.File.RollingInterval,
                    retainedFileCountLimit: sinks.File.RetainedFileCountLimit,
                    restrictedToMinimumLevel: sinks.File.MinimumLevel);
            }

            if (sinks.Database.Enabled)
            {
                // Los errores del propio sink (BD caída, tabla inexistente) se muestran por stderr.
                SelfLog.Enable(Console.Error);

                AddDatabaseSink(configuration, context.Configuration, sinks.Database);
            }
        });

    private static void AddDatabaseSink(
        LoggerConfiguration configuration, IConfiguration appConfiguration, DatabaseSinkOptions options)
    {
        var connectionString = appConfiguration.GetConnectionString(options.ConnectionStringName)
            ?? throw new InvalidOperationException(
                $"No existe la cadena de conexión '{options.ConnectionStringName}' para el log en base de datos.");

        var columnOptions = new ColumnOptions();

        // En lugar del XML "Properties" se guarda el evento completo como JSON,
        // sin repetir las columnas estándar (mensaje, nivel, etc.).
        columnOptions.Store.Remove(StandardColumn.Properties);
        columnOptions.Store.Add(StandardColumn.LogEvent);
        columnOptions.LogEvent.ExcludeStandardColumns = true;

        columnOptions.Id.DataType = SqlDbType.BigInt;
        columnOptions.Level.StoreAsEnum = false;          // texto: "Warning", "Error"...
        columnOptions.TimeStamp.DataType = SqlDbType.DateTime2;
        columnOptions.TimeStamp.ConvertToUtc = true;      // siempre en UTC

        // Columnas propias para filtrar sin abrir el JSON.
        columnOptions.AdditionalColumns =
        [
            new SqlColumn { ColumnName = "Application", PropertyName = "Application",
                            DataType = SqlDbType.NVarChar, DataLength = 100, AllowNull = true },
            new SqlColumn { ColumnName = "TraceId", PropertyName = "TraceId",
                            DataType = SqlDbType.NVarChar, DataLength = 64, AllowNull = true }
        ];

        configuration.WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                SchemaName = options.SchemaName,
                TableName = options.TableName,
                AutoCreateSqlTable = options.AutoCreateTable,
                BatchPostingLimit = options.BatchPostingLimit,
                BatchPeriod = TimeSpan.FromSeconds(options.BatchPeriodSeconds)
            },
            restrictedToMinimumLevel: options.MinimumLevel,
            columnOptions: columnOptions);
    }
}
