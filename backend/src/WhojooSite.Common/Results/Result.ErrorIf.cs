namespace WhojooSite.Common.Results;

public partial class Result<T>
{
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