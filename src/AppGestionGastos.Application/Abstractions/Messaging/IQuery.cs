namespace AppGestionGastos.Application.Abstractions.Messaging;

/// <summary>Consulta de solo lectura. <typeparamref name="TResult"/> es <see cref="Result"/> o <see cref="Result{TValue}"/>.</summary>
public interface IQuery<TResult> where TResult : Result;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : Result
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
