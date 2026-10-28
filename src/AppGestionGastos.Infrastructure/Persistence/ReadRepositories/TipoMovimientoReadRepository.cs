using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.TiposMovimiento;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class TipoMovimientoReadRepository(AppDbContext context) : ITipoMovimientoReadRepository
    {
        public Task<PagedResult<TipoMovimientoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = context.TiposMovimiento.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim();
                query = query.Where(t => t.Nombre.Contains(termino));
            }

            return query
                .OrderBy(t => t.Nombre).ThenBy(t => t.Id)
                .ToPagedResultAsync(
                    page,
                    t => new TipoMovimientoDto(t.Id, t.Nombre, t.Descripcion, t.Naturaleza),
                    cancellationToken);
        }
    }
}
