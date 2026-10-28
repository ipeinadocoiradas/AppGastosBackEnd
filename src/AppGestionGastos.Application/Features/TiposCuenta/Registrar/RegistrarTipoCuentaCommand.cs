namespace AppGestionGastos.Application.Features.TiposCuenta.Crear;

public sealed record RegistrarTipoCuentaCommand(string Nombre, string? Descripcion) : ICommand<Result<TipoCuentaDto>>;