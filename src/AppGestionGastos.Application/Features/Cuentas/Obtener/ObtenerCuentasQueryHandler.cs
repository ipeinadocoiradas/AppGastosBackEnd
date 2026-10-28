using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Domain.Usuarios;

namespace AppGestionGastos.Application.Features.Cuentas.Obtener
{
    internal sealed class ObtenerCuentasQueryHandler(ICuentaReadRepository cuentas, ICurrentUser currentUser)
        : IQueryHandler<ObtenerCuentasQuery, Result<PagedResult<CuentaDto>>>
    {
        public async Task<Result<PagedResult<CuentaDto>>> HandleAsync(ObtenerCuentasQuery query, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } usuarioId) return UsuarioErrors.NoAutenticado;

            return await cuentas.ListarAsync(usuarioId, query.Buscar, query.ToPageRequest(), cancellationToken);
        }        
    }
}
