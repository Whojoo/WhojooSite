namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public Result<T> ErrorIf(Func<T, bool> predicate, ResultError error)
    {
        if (!IsSuccess)
        {
            return this;
        }

        if (predicate.Invoke(_value!))
        {
            WithError(error);
        }

        return this;
    }
}

public partial class Result
{
    public Result ErrorIf(Func<bool> predicate, ResultError error)
    {
        if (predicate.Invoke())
        {
            WithError(error);
        }

        return this;
    }
}

public static class ResultErrorIfExtensions
{
    public static async Task<Result<T>> ErrorIfAsync<T>(
        this Task<Result<T>> result,
        Func<T, bool> predicate,
        ResultError error)
    {
        return (await result).ErrorIf(predicate, error);
    }
}