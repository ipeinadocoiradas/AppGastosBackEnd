using AppGestionGastos.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase(IDispatcher dispatcher) : ControllerBase
{
    protected IDispatcher Dispatcher { get; } = dispatcher;

    /// <summary>200 OK con el valor, o ProblemDetails según el tipo de error.</summary>
    protected IActionResult OkOrProblem<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : Problem(result.Error);

    /// <summary>201 Created con el valor, o ProblemDetails según el tipo de error.</summary>
    protected IActionResult CreatedOrProblem<T>(Result<T> result) =>
        result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : Problem(result.Error);

    /// <summary>204 No Content si tuvo éxito, o ProblemDetails según el tipo de error.</summary>
    protected IActionResult NoContentOrProblem(Result result) =>
        result.IsSuccess ? NoContent() : Problem(result.Error);

    protected IActionResult Problem(Error error)
    {
        var (statusCode, title) = error.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflicto"),
            ErrorType.Unauthorized => (StatusCodes.Status401Unauthorized, "No autenticado"),
            ErrorType.Forbidden => (StatusCodes.Status403Forbidden, "Acceso denegado"),
            _ => (StatusCodes.Status500InternalServerError, "Error del servidor")
        };

        var problem = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: statusCode,
            title: title,
            detail: error.Description);

        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem) { StatusCode = statusCode };
    }
}
