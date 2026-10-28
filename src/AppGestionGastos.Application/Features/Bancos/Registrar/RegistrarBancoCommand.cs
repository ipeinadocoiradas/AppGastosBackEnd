namespace AppGestionGastos.Application.Features.Bancos.Crear;

public sealed record RegistrarBancoCommand(string Nombre, string CodigoInstitucional) : ICommand<Result<BancoDto>>;