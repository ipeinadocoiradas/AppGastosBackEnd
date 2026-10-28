using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace AppGestionGastos.Infrastructure.Persistence.Diagnostics
{
    /// <summary>
    /// Mide el tiempo de cada comando SQL y registra los lentos:
    /// Error si dura >= ErrorThresholdMs, Warning si dura más de WarningThresholdMs.
    /// Usa IOptionsMonitor: los umbrales se pueden cambiar en caliente desde appsettings.
    /// </summary>
    internal sealed class DbCommandTimingInterceptor(
        IOptionsMonitor<DbCommandTimingOptions> optionsMonitor,
        ILogger<DbCommandTimingInterceptor> logger) : DbCommandInterceptor
    {
        // ---- Consultas (SELECT) ----
        public override DbDataReader ReaderExecuted(
            DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
        {
            Analyze(command, eventData);
            return base.ReaderExecuted(command, eventData, result);
        }

        public override ValueTask<DbDataReader> ReaderExecutedAsync(
            DbCommand command, CommandExecutedEventData eventData, DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            Analyze(command, eventData);
            return base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
        }

        // ---- INSERT / UPDATE / DELETE ----
        public override int NonQueryExecuted(
            DbCommand command, CommandExecutedEventData eventData, int result)
        {
            Analyze(command, eventData);
            return base.NonQueryExecuted(command, eventData, result);
        }

        public override ValueTask<int> NonQueryExecutedAsync(
            DbCommand command, CommandExecutedEventData eventData, int result,
            CancellationToken cancellationToken = default)
        {
            Analyze(command, eventData);
            return base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
        }

        // ---- Escalares (COUNT, ANY...) ----
        public override object? ScalarExecuted(
            DbCommand command, CommandExecutedEventData eventData, object? result)
        {
            Analyze(command, eventData);
            return base.ScalarExecuted(command, eventData, result);
        }

        public override ValueTask<object?> ScalarExecutedAsync(
            DbCommand command, CommandExecutedEventData eventData, object? result,
            CancellationToken cancellationToken = default)
        {
            Analyze(command, eventData);
            return base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
        }

        private void Analyze(DbCommand command, CommandExecutedEventData eventData)
        {
            // Un componente de diagnóstico nunca debe romper una consulta.
            try
            {
                var options = optionsMonitor.CurrentValue;
                if (!options.Enabled) return;

                var elapsedMs = eventData.Duration.TotalMilliseconds;

                if (elapsedMs >= options.ErrorThresholdMs)
                {
                    Write(LogLevel.Error, elapsedMs, options.ErrorThresholdMs, command, eventData, options);
                }
                else if (elapsedMs > options.WarningThresholdMs)
                {
                    Write(LogLevel.Warning, elapsedMs, options.WarningThresholdMs, command, eventData, options);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo analizar el tiempo de un comando de base de datos");
            }
        }

        private void Write(
            LogLevel level,
            double elapsedMs,
            int thresholdMs,
            DbCommand command,
            CommandExecutedEventData eventData,
            DbCommandTimingOptions options)
        {
            if (!logger.IsEnabled(level)) return;

            if (!options.IncludeCommandText)
            {
                logger.Log(
                    level,
                    "Consulta a base de datos lenta: {ElapsedMs:F1} ms (umbral {ThresholdMs} ms, origen {CommandSource})",
                    elapsedMs, thresholdMs, eventData.CommandSource);
                return;
            }

            logger.Log(
                level,
                "Consulta a base de datos lenta: {ElapsedMs:F1} ms (umbral {ThresholdMs} ms, origen {CommandSource}). SQL: {CommandText}",
                elapsedMs, thresholdMs, eventData.CommandSource, Normalize(command.CommandText, options.MaxCommandTextLength));
        }

        /// <summary>Compacta saltos de línea y trunca el SQL para no inflar el log.</summary>
        private static string Normalize(string sql, int maxLength)
        {
            var compact = sql.ReplaceLineEndings(" ").Trim();

            return compact.Length <= maxLength ? compact : string.Concat(compact.AsSpan(0, maxLength), "…");
        }
    }
}
