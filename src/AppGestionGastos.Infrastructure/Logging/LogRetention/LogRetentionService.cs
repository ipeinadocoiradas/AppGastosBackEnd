using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Diagnostics;


namespace AppGestionGastos.Infrastructure.Logging.LogRetention
{
    /// <summary>
    /// Elimina de la tabla de logs los registros anteriores a la retención configurada.
    /// Comprueba cada minuto si toca ejecutar y lee la configuración en caliente, de modo que
    /// activar/desactivar el servicio o cambiar la retención no requiere reiniciar.
    /// </summary>
    internal sealed class LogRetentionService(
        IOptionsMonitor<LogRetentionOptions> retentionMonitor,
        IOptionsMonitor<LogSinksOptions> sinksMonitor,
        IConfiguration configuration,
        TimeProvider timeProvider,
        ILogger<LogRetentionService> logger) : BackgroundService
    {
        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan PauseBetweenBatches = TimeSpan.FromMilliseconds(200);
        private const int CommandTimeoutSeconds = 120;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(PollInterval, timeProvider);
            DateTimeOffset? lastRun = null;

            try
            {
                // El primer tick llega tras 1 minuto: deja terminar el arranque (migraciones, seeders).
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        var options = retentionMonitor.CurrentValue;
                        if (!options.Enabled) continue;

                        var now = timeProvider.GetUtcNow();
                        if (lastRun is { } previous && now - previous < TimeSpan.FromHours(options.IntervalHours))
                        {
                            continue;
                        }

                        lastRun = now;
                        await PurgeAsync(options, now, stoppingToken);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        // Un BackgroundService que lanza una excepción detiene toda la aplicación.
                        logger.LogError(ex, "Error al ejecutar la retención de logs; se reintentará en el siguiente ciclo");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Apagado normal de la aplicación.
            }
        }

        private async Task PurgeAsync(LogRetentionOptions options, DateTimeOffset now, CancellationToken cancellationToken)
        {
            var sinks = sinksMonitor.CurrentValue;
            if (!sinks.HasValidLogTable)
            {
                throw new InvalidOperationException("LogSinks:Database: SchemaName o TableName no son identificadores válidos.");
            }

            var database = sinks.Database;
            var connectionString = configuration.GetConnectionString(database.ConnectionStringName)
                ?? throw new InvalidOperationException($"No existe la cadena de conexión '{database.ConnectionStringName}'.");

            // Los logs se guardan en UTC (ConvertToUtc), así que el límite también es UTC.
            var cutoff = now.UtcDateTime.AddMonths(-options.RetentionMonths);

            // Identificadores validados arriba; los valores van como parámetros.
            var sql = $"DELETE TOP (@BatchSize) FROM [{database.SchemaName}].[{database.TableName}] WHERE [TimeStamp] < @Cutoff;";

            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(sql, connection) { CommandTimeout = CommandTimeoutSeconds };
            command.Parameters.Add("@BatchSize", SqlDbType.Int).Value = options.BatchSize;
            command.Parameters.Add("@Cutoff", SqlDbType.DateTime2).Value = cutoff;

            var stopwatch = Stopwatch.StartNew();
            long totalDeleted = 0;
            int deleted;

            do
            {
                deleted = await command.ExecuteNonQueryAsync(cancellationToken);
                totalDeleted += deleted;

                if (deleted == options.BatchSize)
                {
                    await Task.Delay(PauseBetweenBatches, cancellationToken);
                }
            }
            while (deleted == options.BatchSize);

            if (totalDeleted > 0)
            {
                logger.LogInformation(
                    "Retención de logs: eliminados {Deleted} registros anteriores a {Cutoff:u} ({RetentionMonths} meses) en {ElapsedMs} ms",
                    totalDeleted, cutoff, options.RetentionMonths, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                logger.LogDebug("Retención de logs: no había registros anteriores a {Cutoff:u}", cutoff);
            }
        }
    }
}
