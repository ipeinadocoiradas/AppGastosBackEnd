using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Usuarios;


namespace AppGestionGastos.Application.Features.Movimientos.Obtener
{
    internal sealed class ObtenerMovimientosQueryHandler(
     IMovimientoReadRepository movimientos,
     ICuentaRepository cuentas,
     ICurrentUser currentUser)
     : IQueryHandler<ObtenerMovimientosQuery, Result<PagedResult<MovimientoItemDto>>>
    {
        public async Task<Result<PagedResult<MovimientoItemDto>>> HandleAsync(
            ObtenerMovimientosQuery query, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } usuarioId) return UsuarioErrors.NoAutenticado;

            if (query.CuentaId is { } cuentaId)
            {
                var cuenta = await cuentas.GetByIdAsync(cuentaId, cancellationToken);
                if (cuenta is null || cuenta.UsuarioId != usuarioId) return CuentaErrors.NoEncontrada;
            }

            var filtro = new MovimientoFiltro(
                usuarioId, query.CuentaId, query.CategoriaId, query.TipoMovimientoId, query.Desde, query.Hasta);

            return await movimientos.ListarAsync(filtro, query.ToPageRequest(), cancellationToken);
        }
    }
}
