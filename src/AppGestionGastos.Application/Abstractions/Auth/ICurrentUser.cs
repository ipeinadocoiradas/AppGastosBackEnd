namespace AppGestionGastos.Application.Abstractions.Auth;

public interface ICurrentUser
{
    /// <summary>Id del usuario autenticado, o null si la petición es anónima.</summary>
    Guid? UserId { get; }
}
