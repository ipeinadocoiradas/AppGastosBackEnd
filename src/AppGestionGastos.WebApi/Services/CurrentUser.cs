using System.Security.Claims;
using AppGestionGastos.Application.Abstractions.Auth;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AppGestionGastos.WebApi.Services;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? UserId =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id)
            ? id
            : null;
}
