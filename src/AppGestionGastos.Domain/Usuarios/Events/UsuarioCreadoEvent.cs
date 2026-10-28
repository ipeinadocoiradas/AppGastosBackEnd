namespace AppGestionGastos.Domain.Usuarios.Events;

public sealed record UsuarioCreadoEvent(Guid UsuarioId, string Nombre, string Email) : DomainEvent;
