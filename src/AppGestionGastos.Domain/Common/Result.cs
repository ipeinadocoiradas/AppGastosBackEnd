namespace AppGestionGastos.Domain.Common;

/// <summary>Resultado de una operación sin valor de retorno.</summary>
public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        // Invariante de programación: un éxito no puede llevar error y un fallo debe llevarlo.
        if (isSuccess == (error != Error.None))
        {
            throw new ArgumentException("Combinación de éxito/error inválida.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static implicit operator Result(Error error) => Failure(error);
}

/// <summary>Resultado de una operación que devuelve un valor cuando tiene éxito.</summary>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) => _value = value;

    /// <summary>Solo accesible si <see cref="Result.IsSuccess"/> es true.</summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("No se puede acceder al valor de un resultado fallido.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}
