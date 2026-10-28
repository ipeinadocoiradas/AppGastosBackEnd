using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Cuentas;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class CuentaReadRepository(AppDbContext context) : ICuentaReadRepository
    {
        public Task<PagedResult<CuentaDto>> ListarAsync(
            Guid usuarioId, string? buscar, PageRequest page, CancellationToken cancellationToken = default)
        {
            var query =
                         from c in context.Cuentas.AsNoTracking()
                         join b in context.Bancos on c.BancoId equals b.Id
                         join t in context.TiposCuenta on c.TipoCuentaId equals t.Id
                         where c.UsuarioId == usuarioId       
                         select new
                         {
                             c.Id,
                             c.NumeroCuenta,
                             c.SaldoActual,
                             c.BancoId,
                             c.TipoCuentaId,
                             BancoNombre = b.Nombre,
                             TipoCuentaNombre = t.Nombre
                         };

            return query
                .OrderBy(x => x.NumeroCuenta).ThenBy(x => x.Id)
                .ToPagedResultAsync(
                    page,
                    x => new CuentaDto(
                        x.Id, x.NumeroCuenta, x.SaldoActual,
                        x.BancoId, x.BancoNombre,
                        x.TipoCuentaId, x.TipoCuentaNombre),
                    cancellationToken);

        }
    }
}
