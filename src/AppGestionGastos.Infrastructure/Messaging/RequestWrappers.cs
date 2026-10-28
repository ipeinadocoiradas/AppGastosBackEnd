using AppGestionGastos.Application.Abstractions.Messaging;
using FluentValidation;
using FluentValidation.Results;

namespace AppGestionGastos.Infrastructure.Messaging;

internal abstract class RequestWrapper<TResult> where TResult : Result
{
    public abstract Task<TResult> HandleAsync(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

internal sealed class CommandWrapper<TCommand, TResult> : RequestWrapper<TResult>
    where TCommand : ICommand<TResult>
    where TResult : Result
{
    public override async Task<TResult> HandleAsync(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var command = (TCommand)request;

        await RequestValidation.ValidateAsync(command, serviceProvider, cancellationToken);

        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
}

internal sealed class QueryWrapper<TQuery, TResult> : RequestWrapper<TResult>
    where TQuery : IQuery<TResult>
    where TResult : Result
{
    public override async Task<TResult> HandleAsync(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var query = (TQuery)request;

        await RequestValidation.ValidateAsync(query, serviceProvider, cancellationToken);

        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return await handler.HandleAsync(query, cancellationToken);
    }
}

internal static class RequestValidation
{
    /// <summary>
    /// Ejecuta todos los validadores registrados. Si hay fallos lanza <see cref="ValidationException"/>,
    /// que el middleware global transforma en una respuesta ProblemDetails (RFC 7807) por campo.
    /// </summary>
    public static async Task ValidateAsync<TRequest>(
        TRequest request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var validators = serviceProvider.GetServices<IValidator<TRequest>>().ToArray();
        if (validators.Length == 0) return;

        var context = new ValidationContext<TRequest>(request);
        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }
}
