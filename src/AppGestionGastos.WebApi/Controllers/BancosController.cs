using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Bancos;
using AppGestionGastos.Application.Features.Bancos.Actualizar;
using AppGestionGastos.Application.Features.Bancos.Crear;
using AppGestionGastos.Application.Features.Bancos.Eliminar;
using AppGestionGastos.Application.Features.Bancos.Obtener;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[Route("api/bancos")]
[Authorize]
public sealed class BancosController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
{

    /// <summary>Lista los bancos con paginación y búsqueda opcional por nombre o código.</summary>
    [HttpGet]
    [ProducesResponseType<PagedResult<BancoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Obtener(
        [FromQuery] ObtenerBancosQuery query, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.QueryAsync(query, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Registra los bancos.(solo administradores)</summary>
    [HttpPost]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<BancoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(RegistrarBancoCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command, cancellationToken);

        return CreatedOrProblem(result);
    }

    /// <summary>Actualiza los bancos.(solo administradores)</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<BancoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(
    Guid id, ActualizarBancoCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command with { Id = id }, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Elimina un banco.(solo administradores)</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(new EliminarBancoCommand(id), cancellationToken);

        return NoContentOrProblem(result);
    }
}