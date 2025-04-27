namespace WhojooSite.Common;

public class Result<T>
{
    private readonly List<ResultError> _errors = [];
    private readonly bool _isSuccess;
    private readonly T? _value;

    internal Result(T? value, bool isSuccess)
    {
        _value = value;
        _isSuccess = isSuccess;
    }

    internal Result(List<string> errorMessages) : this(errorMessages.Select(x => new ResultError(x, x)).ToList())
    {
    }

    internal Result(List<ResultError> errorResults) : this(default, false)
    {
        _errors = errorResults;
    }

    public TReturn Match<TReturn>(Func<T, TReturn> onSuccess, Func<List<ResultError>, TReturn> onError)
    {
        return _isSuccess ? onSuccess(_value!) : onError(_errors);
    }
}

public class Result
{
    private readonly List<ResultError> _errors = [];
    private readonly bool _isSuccess;

    private Result(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    private Result(List<string> errorMessages) : this(errorMessages.Select(x => new ResultError(x, x)).ToList())
    {
    }

    private Result(List<ResultError> errorResults) : this(false)
    {
        _errors = errorResults;
    }

    public TReturn Match<TReturn>(Func<TReturn> onSuccess, Func<List<ResultError>, TReturn> onError)
    {
        return _isSuccess ? onSuccess() : onError(_errors);
    }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true);
    }

    public static Result Failure()
    {
        return new Result(false);
    }

    public static Result<T> Failure<T>()
    {
        return new Result<T>(default, false);
    }

    public static Result Failure(string errorMessage)
    {
        return new Result([errorMessage]);
    }

    public static Result<T> Failure<T>(string errorMessage)
    {
        return new Result<T>(default, false);
    }

    public static Result Failure(List<string> errorMessages)
    {
        return new Result(errorMessages);
    }

    public static Result Failure(IEnumerable<string> errorMessages)
    {
        return new Result(errorMessages.ToList());
    }

    public static Result<T> Failure<T>(List<string> errorMessages)
    {
        return new Result<T>(errorMessages);
    }

    public static Result<T> Failure<T>(IEnumerable<string> errorMessages)
    {
        return new Result<T>(errorMessages.ToList());
    }

    public static Result Failure(List<ResultError> errorMessages)
    {
        return new Result(errorMessages);
    }

    public static Result Failure(IEnumerable<ResultError> errorMessages)
    {
        return new Result(errorMessages.ToList());
    }

    public static Result<T> Failure<T>(List<ResultError> errorMessages)
    {
        return new Result<T>(errorMessages);
    }

    public static Result<T> Failure<T>(IEnumerable<ResultError> errorMessages)
    {
        return new Result<T>(errorMessages.ToList());
    }
}

public record ResultError(string Code, List<string> Description)
{
    public ResultError(string code, string description) : this(code, [description]) { }
    public ResultError(string code, IEnumerable<string> descriptions) : this(code, descriptions.ToList()) { }
}

public enum ResultStatus
{
    Unspecified,
    BadRequest,
    NotFound,
    InternalError
}