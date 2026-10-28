using AppGestionGastos.Application.Abstractions.Messaging;
using AppGestionGastos.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppGestionGastos.WebApi.Controllers;

/// <summary>Base de los controladores de administración: exige el rol Admin en todos sus endpoints.</summary>
[Authorize(Policy = Policies.AdminOnly)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
public abstract class AdminControllerBase(IDispatcher dispatcher) : ApiControllerBase(dispatcher);