using System.ComponentModel.DataAnnotations;

namespace AppGestionGastos.Infrastructure.Logging.LogRetention
{
    public sealed class LogRetentionOptions
    {
        public const string SectionName = "LogRetention";

        /// <summary>Interruptor: si es false el servicio no borra nada. Por defecto desactivado (opt-in).</summary>
        public bool Enabled { get; init; } = false;

        /// <summary>Meses de logs que se conservan; lo anterior se elimina.</summary>
        [Range(1, 120)]
        public int RetentionMonths { get; init; } = 3;

        /// <summary>Cada cuántas horas se ejecuta la limpieza.</summary>
        [Range(1, 168)]
        public int IntervalHours { get; init; } = 24;

        /// <summary>Filas por lote de borrado (evita transacciones enormes y bloqueos largos).</summary>
        [Range(100, 100_000)]
        public int BatchSize { get; init; } = 5000;
    }
}
