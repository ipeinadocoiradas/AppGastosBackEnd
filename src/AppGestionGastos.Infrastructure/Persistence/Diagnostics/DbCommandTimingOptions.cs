using System.ComponentModel.DataAnnotations;


namespace AppGestionGastos.Infrastructure.Persistence.Diagnostics
{
    public sealed class DbCommandTimingOptions
    {
        public const string SectionName = "DbCommandTiming";

        /// <summary>Interruptor general: si es false no se registra nada.</summary>
        public bool Enabled { get; init; } = true;

        /// <summary>Por encima de este tiempo (ms) se registra un Warning.</summary>
        [Range(1, int.MaxValue)]
        public int WarningThresholdMs { get; init; } = 1000;

        /// <summary>Con este tiempo o más (ms) se registra un Error.</summary>
        [Range(1, int.MaxValue)]
        public int ErrorThresholdMs { get; init; } = 3000;

        /// <summary>Incluye el texto SQL en el log (nunca los valores de los parámetros).</summary>
        public bool IncludeCommandText { get; init; } = true;

        [Range(50, 20_000)]
        public int MaxCommandTextLength { get; init; } = 1000;
    }
}
