using AppGestionGastos.Application.Abstractions.Auth;
using AppGestionGastos.Domain.Cuentas;
using AppGestionGastos.Domain.Usuarios;


namespace AppGestionGastos.Application.Features.Cuentas.Actualizar
{
    internal sealed class ActualizarCuentaCommandHandler(
     ICuentaRepository cuentas,
     IBancoRepository bancos,
     ITipoCuentaRepository tiposCuenta,
     ICurrentUser currentUser,
     IUnitOfWork unitOfWork)
     : ICommandHandler<ActualizarCuentaCommand, Result<CuentaDto>>
    {
        public async Task<Result<CuentaDto>> HandleAsync(
            ActualizarCuentaCommand command, CancellationToken cancellationToken = default)
        {
            if (currentUser.UserId is not { } usuarioId) return UsuarioErrors.NoAutenticado;

            var cuenta = await cuentas.GetByIdAsync(command.Id, cancellationToken);
            if (cuenta is null || cuenta.UsuarioId != usuarioId) return CuentaErrors.NoEncontrada;   // ajena = 404

            // Validan existencia y aportan los nombres para el DTO.
            var banco = await bancos.GetByIdAsync(command.BancoId, cancellationToken);
            if (banco is null) return CuentaErrors.BancoNoEncontrado;

            var tipoCuenta = await tiposCuenta.GetByIdAsync(command.TipoCuentaId, cancellationToken);
            if (tipoCuenta is null) return CuentaErrors.TipoCuentaNoEncontrado;

            var numero = command.NumeroCuenta.Trim();
            if (await cuentas.ExisteNumeroEnOtraCuentaAsync(cuenta.Id, banco.Id, numero, cancellationToken))
            {
                return CuentaErrors.NumeroYaRegistrado;
            }

            var resultado = cuenta.Actualizar(numero, banco.Id, tipoCuenta.Id);
            if (resultado.IsFailure) return resultado.Error;

            var commit = await unitOfWork.CommitAsync(cancellationToken);
            if (commit.IsFailure) return commit.Error;

            return new CuentaDto(
                cuenta.Id, cuenta.NumeroCuenta, cuenta.SaldoActual,
                banco.Id, banco.Nombre,
                tipoCuenta.Id, tipoCuenta.Nombre);
        }
    }
}
