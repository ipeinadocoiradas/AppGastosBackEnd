using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Categorias;
using AppGestionGastos.Application.Features.Categorias.Crear;
using AppGestionGastos.Application.Features.Cuentas;
using AppGestionGastos.Application.Features.Cuentas.Actualizar;
using AppGestionGastos.Application.Features.Cuentas.Obtener;
using AppGestionGastos.Application.Features.Cuentas.Registrar;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers
{
    [Route("api/cuentas")]
    [Authorize]
    public class CuentasController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
    {
        /// <summary>
        /// Obtiene las cuentas paginadas de un usuario
        /// </summary>        
        [HttpGet]
        [ProducesResponseType<PagedResult<CuentaDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Obtener(
        [FromQuery] ObtenerCuentasQuery query, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.QueryAsync(query, cancellationToken);

            return OkOrProblem(result);
        }

        /// <summary>Registra cuentas bancarias de un usuario.</summary>
        [HttpPost]        
        [ProducesResponseType<CuentaDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Crear(RegistrarCuentaCommand command, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.SendAsync(command, cancellationToken);

            return CreatedOrProblem(result);
        }

        /// <summary>Actualiza la cuenta bancaria de un usuario.</summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType<CuentaDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Actualizar(Guid id, ActualizarCuentaCommand command, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.SendAsync(command with { Id = id }, cancellationToken);

            return OkOrProblem(result);
        }
    }
}
