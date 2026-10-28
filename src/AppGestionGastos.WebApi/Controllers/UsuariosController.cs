using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Usuarios;
using AppGestionGastos.Application.Features.Usuarios.Actualizar;
using AppGestionGastos.Application.Features.Usuarios.Eliminar;
using AppGestionGastos.Application.Features.Usuarios.Obtener;
using AppGestionGastos.Application.Features.Usuarios.Registrar;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

[Route("api/usuarios")]
public sealed class UsuariosController(IDispatcher dispatcher) : ApiControllerBase(dispatcher)
{
    /// <summary>Lista usuarios (solo administradores)</summary>
    [HttpGet]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<PagedResult<UsuarioListadoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Obtener(
        [FromQuery] ObtenerUsuariosQuery query, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.QueryAsync(query, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Registra un nuevo usuario (rol "Usuario").</summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType<UsuarioDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command, cancellationToken);

        return CreatedOrProblem(result);
    }

    /// <summary>Actualiza un usuario (rol "Usuario").</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType<UsuarioListadoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(
    Guid id, ActualizarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(command with { Id = id }, cancellationToken);

        return OkOrProblem(result);
    }

    /// <summary>Elimina un usuario (rol "Usuario").</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var result = await Dispatcher.SendAsync(new EliminarUsuarioCommand(id), cancellationToken);

        return NoContentOrProblem(result);
    }
}
