using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Usuarios;


namespace AppGestionGastos.Application.Features.Cuentas.Registrar
{
    internal sealed class RegistrarCuentaCommandHandler(
        ICurrentUser currentUser,
        IBancoRepository bancos,
        ITipoCuentaRepository tiposCuenta,
        ICuentaRepository cuentas,
        IUnitOfWork unitOfWork) : ICommandHandler<RegistrarCuentaCommand, Result<CuentaDto>>
    {
        public async Task<Result<CuentaDto>> HandleAsync(RegistrarCuentaCommand command, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } usuarioId) return UsuarioErrors.NoAutenticado;

            var banco = await bancos.GetByIdAsync(command.BancoId, cancellationToken);
            if (banco is null) return CuentaErrors.BancoNoEncontrado;

            var tipoCuenta = await tiposCuenta.GetByIdAsync(command.TipoCuentaId, cancellationToken);
            if (tipoCuenta is null) return CuentaErrors.TipoCuentaNoEncontrado;

            var numero = command.NumeroCuenta.Trim();
            if (await cuentas.ExisteNumeroAsync(banco.Id, numero, cancellationToken))
            {
                return CuentaErrors.NumeroYaRegistrado;
            }

            var cuentaResult = Cuenta.Crear(command.NumeroCuenta, usuarioId, command.BancoId, command.TipoCuentaId, command.SaldoActual);
            if (cuentaResult.IsFailure) return cuentaResult.Error;

            var cuenta = cuentaResult.Value;

            cuentas.Add(cuenta);

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new CuentaDto(
            cuenta.Id, cuenta.NumeroCuenta, cuenta.SaldoActual,
            banco.Id, banco.Nombre,
            tipoCuenta.Id, tipoCuenta.Nombre);
        }
    }
}
