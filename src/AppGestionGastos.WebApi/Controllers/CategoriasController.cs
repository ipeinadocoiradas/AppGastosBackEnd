using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Categorias;
using AppGestionGastos.Application.Features.Categorias.Actualizar;
using AppGestionGastos.Application.Features.Categorias.Crear;
using AppGestionGastos.Application.Features.Categorias.Eliminar;
using AppGestionGastos.Application.Features.Categorias.Obtener;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[Route("api/categorias")]
[Authorize]
public sealed class CategoriasController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
{
    /// <summary>Lista las categorías paginadas.</summary>
    [HttpGet]
    [ProducesResponseType<PagedResult<CategoriaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Obtener(
       [FromQuery] ObtenerCategoriasQuery query, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.QueryAsync(query, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Registra las categorías.</summary>
    [HttpPost]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<CategoriaDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(RegistrarCategoriaCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command, cancellationToken);

        return CreatedOrProblem(result);
    }

    /// <summary>Actualiza categoría.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<CategoriaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(
    Guid id, ActualizarCategoriaCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command with { Id = id }, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Elimina categoría.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(new EliminarCategoriaCommand(id), cancellationToken);

        return NoContentOrProblem(result);
    }
}