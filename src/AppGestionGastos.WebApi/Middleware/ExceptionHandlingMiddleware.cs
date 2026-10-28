using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace AppGestionGastos.WebApi.Middleware;

/// <summary>
/// Captura las excepciones no controladas y las transforma en respuestas RFC 7807 (ProblemDetails).
/// Los fallos de FluentValidation se devuelven como ValidationProblemDetails con los errores agrupados por campo.
/// </summary>
/// 
public partial class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private const int ClientClosedRequest = 499;
    private const string ProblemContentType = "application/problem+json";
    private static readonly JsonSerializerOptions ProblemJsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>Cuerpo propio de un error de validación (formato RFC 7807 con 'errors' por campo).</summary>
    private sealed record ValidationProblemBody(
        string Type, string Title, int Status, string Detail, string TraceId,
        Dictionary<string, string[]> Errors);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            logger.LogInformation("Petición {Method} {Path} cancelada por el cliente", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = ClientClosedRequest;
        }
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            Log(exception, context);

            if (exception is ValidationException validation)
            {
                await WriteValidationProblemAsync(context, validation);
                return;
            }

            await MapToProblem(exception).ExecuteAsync(context);
        }
    }

    private static Task WriteValidationProblemAsync(HttpContext context, ValidationException exception)
    {
        var errors = GroupErrorsByField(exception);

        var body = new ValidationProblemBody(
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title: "Errores de validación",
            Status: StatusCodes.Status400BadRequest,
            // Los mensajes reales también en 'detail', para clientes que solo muestran este campo.
            Detail: string.Join(" ", errors.Values.SelectMany(messages => messages)),
            TraceId: Activity.Current?.Id ?? context.TraceIdentifier,
            Errors: errors);

        context.Response.StatusCode = body.Status;

        return context.Response.WriteAsJsonAsync(
            body, ProblemJsonOptions, ProblemContentType, context.RequestAborted);
    }

    private static IResult MapToProblem(Exception exception) => exception switch
    {
        DbUpdateConcurrencyException => Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            title: "Conflicto de concurrencia",
            detail: "El recurso fue modificado por otra operación. Reintente la solicitud."),

        DbUpdateException { InnerException: SqlException { Number: 547 } } => Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            title: "Operación no permitida",
            detail: "El registro está en uso o referencia un dato que no existe."),

        DbUpdateException { InnerException: SqlException { Number: 2601 or 2627 } } => Results.Problem(
            statusCode: StatusCodes.Status409Conflict,
            title: "Registro duplicado",
            detail: "Ya existe un registro con los mismos valores únicos."),

        _ => Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Error interno del servidor",
            detail: "Ocurrió un error inesperado. Si persiste, contacte con soporte indicando el traceId.")
    };

    private static Dictionary<string, string[]> GroupErrorsByField(ValidationException exception) =>
        exception.Errors
            .GroupBy(
                failure => JsonNamingPolicy.CamelCase.ConvertName(failure.PropertyName),
                failure => failure.ErrorMessage)
            .ToDictionary(group => group.Key, group => group.Distinct().ToArray());

    private void Log(Exception exception, HttpContext context)
    {
        if (exception is ValidationException validation)
        {
            // Solo campo y mensaje: nunca AttemptedValue (podría ser una contraseña o un email).
            var errores = string.Join(
                "; ", validation.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

            logger.LogWarning(
                "Validación fallida en {Method} {Path}: {Errors}",
                context.Request.Method, context.Request.Path, errores);
            return;
        }

        logger.LogError(
            exception, "Excepción no controlada en {Method} {Path}",
            context.Request.Method, context.Request.Path);
    }
}
