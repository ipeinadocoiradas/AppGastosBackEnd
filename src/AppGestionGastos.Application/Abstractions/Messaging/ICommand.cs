namespace AppGestionGastos.Application.Abstractions.Messaging;

/// <summary>
/// Comando que modifica estado. <typeparamref name="TResult"/> es <see cref="Result"/> o <see cref="Result{TValue}"/>.
/// </summary>
public interface ICommand<TResult> where TResult : Result;

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : Result
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
