using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposCuenta;
using AppGestionGastos.Application.Features.TiposCuenta.Actualizar;
using AppGestionGastos.Application.Features.TiposCuenta.Crear;
using AppGestionGastos.Application.Features.TiposCuenta.Eliminar;
using AppGestionGastos.Application.Features.TiposCuenta.Obtener;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[Route("api/tipos-cuenta")]
[Authorize]
public sealed class TiposCuentaController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
{
    /// <summary>Lista los tipos de cuentas paginadas.</summary>
    [HttpGet]
    [ProducesResponseType<PagedResult<TipoCuentaDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Obtener(
       [FromQuery] ObtenerTiposCuentaQuery query, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.QueryAsync(query, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Registra los tipos de cuentas.</summary>
    [HttpPost]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<TipoCuentaDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Crear(RegistrarTipoCuentaCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command, cancellationToken);

        return CreatedOrProblem(result);
    }

    /// <summary>Actualiza tipo de cuenta.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<TipoCuentaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(
    Guid id, ActualizarTipoCuentaCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command with { Id = id }, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Elimina tipo de cuenta.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(new EliminarTipoCuentaCommand(id), cancellationToken);

        return NoContentOrProblem(result);
    }
}