using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Movimientos;
using AppGestionGastos.Application.Features.Movimientos.Obtener;
using AppGestionGastos.Application.Features.Movimientos.Registrar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[Route("api/movimientos")]
[Authorize]
public sealed class MovimientosController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
{

    /// <summary>Lista los movimientos del usuario autenticado, del más reciente al más antiguo.</summary>
    [HttpGet]
    [ProducesResponseType<PagedResult<MovimientoItemDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obtener(
        [FromQuery] ObtenerMovimientosQuery query, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.QueryAsync(query, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Registra un movimiento en una cuenta del usuario autenticado y actualiza su saldo.</summary>
    [HttpPost]
    [ProducesResponseType<MovimientoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(RegistrarMovimientoCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command, cancellationToken);

        return CreatedOrProblem(result);
    }
}
