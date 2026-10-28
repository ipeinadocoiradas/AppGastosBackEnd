using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Usuarios;
using AppGestionGastos.Domain.Usuarios;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;

namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class UsuarioReadRepository(AppDbContext context) : IUsuarioReadRepository
    {
        public Task<PagedResult<UsuarioListadoDto>> ListarAsync(
            string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query = context.Usuarios.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim();
                var emailResult = Email.Create(termino);

                if (emailResult.IsSuccess)
                {
                    // Email completo: coincidencia exacta (la columna usa un conversor de valor)
                    // o nombre parcial.
                    var email = emailResult.Value;
                    query = query.Where(u => u.Email == email || u.Nombre.Contains(termino));
                }
                else
                {
                    query = query.Where(u => u.Nombre.Contains(termino));
                }
            }

            return query
                .OrderBy(u => u.Nombre).ThenBy(u => u.Id)
                .ToPagedResultAsync(
                    page,
                    u => new UsuarioListadoDto(u.Id, u.Nombre, u.Email.Value, u.Roles),
                    cancellationToken);
        }
    }
}
