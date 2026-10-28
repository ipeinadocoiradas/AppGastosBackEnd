using AppGestionGastos.Application.Common.Pagination;


namespace AppGestionGastos.Application.Features.Usuarios.Obtener
{
   internal sealed class ObtenerUsuariosQueryHandler(IUsuarioReadRepository usuarios)
    : IQueryHandler<ObtenerUsuariosQuery, Result<PagedResult<UsuarioListadoDto>>>
    {
        public async Task<Result<PagedResult<UsuarioListadoDto>>> HandleAsync(
            ObtenerUsuariosQuery query, CancellationToken cancellationToken = default) =>
            await usuarios.ListarAsync(query.Buscar, query.ToPageRequest(), cancellationToken);
    }
}
