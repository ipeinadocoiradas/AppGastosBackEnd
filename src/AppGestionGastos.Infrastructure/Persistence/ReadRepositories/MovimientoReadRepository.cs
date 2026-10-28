using AppGestionGastos.Application.Common.Pagination;
using AppGestionGastos.Application.Features.Movimientos;
using AppGestionGastos.Infrastructure.Persistence.Pagination;
using Microsoft.EntityFrameworkCore;


namespace AppGestionGastos.Infrastructure.Persistence.ReadRepositories
{
    internal sealed class MovimientoReadRepository(AppDbContext context) : IMovimientoReadRepository
    {
        public Task<PagedResult<MovimientoItemDto>> ListarAsync(
            MovimientoFiltro filtro, PageRequest page, CancellationToken cancellationToken = default)
        {
            // Las entidades no tienen propiedades de navegación: los joins son explícitos.
            // El filtro por usuario garantiza que solo se ven movimientos de cuentas propias.
            var query =
                from m in context.Movimientos.AsNoTracking()
                join c in context.Cuentas on m.CuentaId equals c.Id
                join cat in context.Categorias on m.CategoriaId equals cat.Id
                join t in context.TiposMovimiento on m.TipoMovimientoId equals t.Id
                where c.UsuarioId == filtro.UsuarioId
                select new
                {
                    Movimiento = m,
                    c.NumeroCuenta,
                    Categoria = cat.Nombre,
                    TipoMovimiento = t.Nombre,
                    t.Naturaleza
                };

            if (filtro.CuentaId is { } cuentaId)
                query = query.Where(x => x.Movimiento.CuentaId == cuentaId);

            if (filtro.CategoriaId is { } categoriaId)
                query = query.Where(x => x.Movimiento.CategoriaId == categoriaId);

            if (filtro.TipoMovimientoId is { } tipoId)
                query = query.Where(x => x.Movimiento.TipoMovimientoId == tipoId);

            if (filtro.Desde is { } desde)
                query = query.Where(x => x.Movimiento.FechaHora >= desde);

            if (filtro.Hasta is { } hasta)
                query = query.Where(x => x.Movimiento.FechaHora <= hasta);

            return query
                .OrderByDescending(x => x.Movimiento.FechaHora)
                .ThenByDescending(x => x.Movimiento.Id)
                .ToPagedResultAsync(
                    page,
                    x => new MovimientoItemDto(
                        x.Movimiento.Id,
                        x.Movimiento.CuentaId,
                        x.NumeroCuenta,
                        x.Movimiento.CategoriaId,
                        x.Categoria,
                        x.Movimiento.TipoMovimientoId,
                        x.TipoMovimiento,
                        x.Naturaleza,
                        x.Movimiento.Monto,
                        x.Movimiento.FechaHora,
                        x.Movimiento.Descripcion),
                    cancellationToken);
        }
    }
}
