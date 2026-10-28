using AppGestionGastos.Application.Abstractions.DomainEvents;
using AppGestionGastos.Domain.Usuarios.Events;
using Microsoft.Extensions.Logging;

internal sealed class UsuarioCreadoEventHandler(ILogger<UsuarioCreadoEventHandler> logger)
    : IPostCommitDomainEventHandler<UsuarioCreadoEvent>
{
    public Task HandleAsync(UsuarioCreadoEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Email de bienvenida enviado (simulado) a {Email} para el usuario {UsuarioId}",
            domainEvent.Email, domainEvent.UsuarioId);

        return Task.CompletedTask;
    }
}