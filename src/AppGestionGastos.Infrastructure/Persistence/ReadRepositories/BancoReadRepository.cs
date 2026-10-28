using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Bancos;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class BancoReadRepository(AppDbContext context) : IBancoReadRepository
    {
        public Task<PagedResult<BancoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = context.Bancos.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim();
                query = query.Where(b => b.Nombre.Contains(termino)
                                      || b.CodigoInstitucional.Contains(termino));
            }

            return query
                .OrderBy(b => b.Nombre).ThenBy(b => b.Id)
                .ToPagedResultAsync(
                    page, b => new BancoDto(b.Id, b.Nombre, b.CodigoInstitucional), cancellationToken);
        }
    }
}
