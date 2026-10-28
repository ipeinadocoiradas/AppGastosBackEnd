using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposMovimiento;
using AppGestionGastos.Application.Features.TiposMovimiento.Obtener;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers
{
    [Route("api/tipos-movimiento")]
    [Authorize]
    public sealed class TiposMovimientoController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
    {
        /// <summary>Lista los tipos de movimiento (ingreso/egreso) para cargar selectores en el cliente.</summary>
        [HttpGet]
        [ProducesResponseType<PagedResult<TipoMovimientoDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Obtener(
            [FromQuery] ObtenerTiposMovimientoQuery query, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.QueryAsync(query, cancellationToken);

            return OkOrProblem(result);
        }
    }
}
