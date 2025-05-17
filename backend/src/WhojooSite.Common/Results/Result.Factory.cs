namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, []);
    }

    public static Result<T> Failure()
    {
        return new Result<T>(ResultError.Failure());
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(ResultError.Failure(description: errorMessage));
    }

    public static Result<T> Failure(ResultError error)
    {
        return new Result<T>(error);
    }

    public static Result<T> Failure(List<ResultError> errors)
    {
        return new Result<T>(errors);
    }
}

public partial class Result
{
    public static Result Success()
    {
        return new Result([]);
    }

    public static Result Failure()
    {
        return new Result(ResultError.Failure());
    }

    public static Result Failure(string errorMessage)
    {
        return new Result(ResultError.Failure(description: errorMessage));
    }

    public static Result Failure(ResultError error)
    {
        return new Result(error);
    }

    public static Result Failure(List<ResultError> errors)
    {
        return new Result(errors);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, []);
    }

    public static Result<T> Failure<T>()
    {
        return new Result<T>(ResultError.Failure());
    }

    public static Result<T> Failure<T>(string errorMessage)
    {
        return new Result<T>(ResultError.Failure(description: errorMessage));
    }

    public static Result<T> Failure<T>(ResultError error)
    {
        return new Result<T>(error);
    }

    public static Result<T> Failure<T>(List<ResultError> errors)
    {
        return new Result<T>(errors);
    }
}