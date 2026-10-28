using System.Collections.Concurrent;
using AppGestionGastos.Application.Abstractions.Messaging;

namespace AppGestionGastos.Infrastructure.Messaging;

/// <summary>
/// Dispatcher CQRS propio y tipado. Resuelve el handler por tipo de comando/consulta,
/// ejecuta las validaciones de FluentValidation y devuelve el <see cref="Result"/> del handler.
/// Los wrappers se cachean por tipo para evitar reflexión en cada llamada.
/// </summary>
internal sealed class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private static readonly ConcurrentDictionary<Type, object> CommandWrappers = new();
    private static readonly ConcurrentDictionary<Type, object> QueryWrappers = new();

    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : Result
    {
        ArgumentNullException.ThrowIfNull(command);

        var wrapper = (RequestWrapper<TResult>)CommandWrappers.GetOrAdd(
            command.GetType(),
            static commandType => Activator.CreateInstance(
                typeof(CommandWrapper<,>).MakeGenericType(commandType, typeof(TResult)))!);

        return wrapper.HandleAsync(command, serviceProvider, cancellationToken);
    }

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : Result
    {
        ArgumentNullException.ThrowIfNull(query);

        var wrapper = (RequestWrapper<TResult>)QueryWrappers.GetOrAdd(
            query.GetType(),
            static queryType => Activator.CreateInstance(
                typeof(QueryWrapper<,>).MakeGenericType(queryType, typeof(TResult)))!);

        return wrapper.HandleAsync(query, serviceProvider, cancellationToken);
    }
}
