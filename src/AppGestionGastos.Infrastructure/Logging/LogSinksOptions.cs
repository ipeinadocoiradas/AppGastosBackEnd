using Serilog;
using Serilog.Events;
using System.Text.RegularExpressions;

namespace AppGestionGastos.Infrastructure.Logging
{
    public sealed class LogSinksOptions
    {
        public const string SectionName = "LogSinks";
        public ConsoleSinkOptions Console { get; init; } = new();
        public FileSinkOptions File { get; init; } = new();
        public DatabaseSinkOptions Database { get; init; } = new();
        
        /// <summary>Esquema y tabla de logs son identificadores simples (seguros para usar en SQL).</summary>
        public bool HasValidLogTable =>
            Identifier.IsMatch(Database.SchemaName) && Identifier.IsMatch(Database.TableName);

        /// <summary>Valida la configuración al arrancar: un error aquí es de configuración, no de negocio.</summary>
        public void Validate()
        {
            if (File.Enabled && string.IsNullOrWhiteSpace(File.Path))
                throw new InvalidOperationException("LogSinks:File:Path es obligatorio cuando File está activado.");

            if (!Database.Enabled) return;

            if (string.IsNullOrWhiteSpace(Database.ConnectionStringName))
                throw new InvalidOperationException("LogSinks:Database:ConnectionStringName es obligatorio.");

            if (Database.BatchPostingLimit is < 1 or > 1000)
                throw new InvalidOperationException("LogSinks:Database:BatchPostingLimit debe estar entre 1 y 1000.");

            if (Database.BatchPeriodSeconds < 1)
                throw new InvalidOperationException("LogSinks:Database:BatchPeriodSeconds debe ser al menos 1.");

            // El nombre de tabla y esquema se usan en SQL: solo identificadores simples.
            if (!Identifier.IsMatch(Database.SchemaName) || !Identifier.IsMatch(Database.TableName))
                throw new InvalidOperationException("LogSinks:Database: SchemaName y TableName solo admiten letras, números y '_'.");
        }

        private static readonly Regex Identifier = new("^[A-Za-z_][A-Za-z0-9_]*$", RegexOptions.Compiled);
    }

    public sealed class ConsoleSinkOptions
    {
        public bool Enabled { get; init; } = true;
        public LogEventLevel MinimumLevel { get; init; } = LogEventLevel.Information;
    }

    public sealed class FileSinkOptions
    {
        public bool Enabled { get; init; } = true;
        public string Path { get; init; } = "logs/app-.log";
        public RollingInterval RollingInterval { get; init; } = RollingInterval.Day;
        public int RetainedFileCountLimit { get; init; } = 14;
        public LogEventLevel MinimumLevel { get; init; } = LogEventLevel.Information;
    }

    public sealed class DatabaseSinkOptions
    {
        public bool Enabled { get; init; } = false;

        /// <summary>Nombre dentro de ConnectionStrings (puede ser la misma BD de la aplicación u otra).</summary>
        public string ConnectionStringName { get; init; } = "DefaultConnection";

        public string SchemaName { get; init; } = "dbo";
        public string TableName { get; init; } = "Logs";

        /// <summary>Solo para desarrollo: la BD debe existir ya al arrancar. Lo normal es usar la migración.</summary>
        public bool AutoCreateTable { get; init; } = false;

        public LogEventLevel MinimumLevel { get; init; } = LogEventLevel.Warning;
        public int BatchPostingLimit { get; init; } = 100;
        public int BatchPeriodSeconds { get; init; } = 5;
    }
}
