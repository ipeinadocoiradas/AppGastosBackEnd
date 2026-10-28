using FluentValidation;


namespace AppGestionGastos.Application.Common.Pagination
{
    /// <summary>Base de los validadores de consultas paginadas: hereda y añade tus propias reglas.</summary>
    public abstract class PagedQueryValidator<TQuery> : AbstractValidator<TQuery>
        where TQuery : IPagedQuery
    {
        protected PagedQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, PageRequest.MaxPageSize);
        }
    }
}
