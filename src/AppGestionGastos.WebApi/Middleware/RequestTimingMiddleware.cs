using System.Diagnostics;
using System.Globalization;

namespace AppGestionGastos.WebApi.Middleware;

/// <summary>Mide el tiempo exacto de cada petición HTTP, lo registra en Serilog y lo expone en una cabecera.</summary>

public partial class RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
{
    private const string HeaderName = "X-Response-Time-Ms";
    private static readonly TimeSpan SlowRequestThreshold = TimeSpan.FromMilliseconds(1000); //1 segundo

    // --- Delegados LoggerMessage eficientes ---
    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Warning,
        Message = "Petición lenta: HTTP {Method} {Path} respondió {StatusCode} en {ElapsedMs:F4} ms")]
    private partial void LogPeticionLenta(string method, string? path, int statusCode, double elapsedMs);

    [LoggerMessage(
        EventId = 102,
        Level = LogLevel.Information,
        Message = "HTTP {Method} {Path} respondió {StatusCode} en {ElapsedMs:F4} ms")]
    private partial void LogPeticionNormal(string method, string? path, int statusCode, double elapsedMs);


    public async Task InvokeAsync(HttpContext context)
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        context.Response.OnStarting(() =>
        {
            var elapsedMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
            context.Response.Headers[HeaderName] = elapsedMs.ToString("F4", CultureInfo.InvariantCulture);
            return Task.CompletedTask;
        });

        try
        {
            await next(context);
        }
        finally
        {
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            // CLAVE CA1873: Solo extraemos y evaluamos las propiedades de 'context' si el log correspondiente está activo
            if (elapsed > SlowRequestThreshold)
            {
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    LogPeticionLenta(
                        context.Request.Method,
                        context.Request.Path.Value,
                        context.Response.StatusCode,
                        elapsed.TotalMilliseconds);
                }
            }
            else
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    LogPeticionNormal(
                        context.Request.Method,
                        context.Request.Path.Value,
                        context.Response.StatusCode,
                        elapsed.TotalMilliseconds);
                }
            }
        }
    }
}
