using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Categorias;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{

    internal sealed class CategoriaReadRepository(AppDbContext context) : ICategoriaReadRepository
    {
        public Task<PagedResult<CategoriaDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = context.Categorias.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim();
                query = query.Where(c => c.Nombre.Contains(termino));
            }

            return query
                .OrderBy(c => c.Nombre).ThenBy(c => c.Id)
                .ToPagedResultAsync(
                    page, c => new CategoriaDto(c.Id, c.Nombre, c.Descripcion), cancellationToken);
        }
    }
}
