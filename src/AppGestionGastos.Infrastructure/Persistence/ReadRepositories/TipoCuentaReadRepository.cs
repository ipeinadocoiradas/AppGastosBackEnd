using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposCuenta;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class TipoCuentaReadRepository(AppDbContext context) : ITipoCuentaReadRepository
    {
        public Task<PagedResult<TipoCuentaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = context.TiposCuenta.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim();
                query = query.Where(t => t.Nombre.Contains(termino));
            }

            return query
                .OrderBy(t => t.Nombre).ThenBy(t => t.Id)
                .ToPagedResultAsync(
                    page, t => new TipoCuentaDto(t.Id, t.Nombre, t.Descripcion), cancellationToken);
        }
    }
}
