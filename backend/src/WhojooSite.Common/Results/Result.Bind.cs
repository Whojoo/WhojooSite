namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public Result<TBindValue> Bind<TBindValue>(Func<T, Result<TBindValue>> binder)
    {
        return IsSuccess ? binder(_value!) : Result<TBindValue>.Failure(_errors);
    }

    public async Task<Result<TBindValue>> BindAsync<TBindValue>(Func<T, Task<Result<TBindValue>>> binder)
    {
        return IsSuccess ? await binder(_value!) : Result<TBindValue>.Failure(_errors);
    }
}

public partial class Result
{
    public Result<TBindValue> Bind<TBindValue>(Func<Result<TBindValue>> binder)
    {
        return IsSuccess ? binder() : Result<TBindValue>.Failure(_errors);
    }

    public async Task<Result<TBindValue>> BindAsync<TBindValue>(Func<Task<Result<TBindValue>>> binder)
    {
        return IsSuccess ? await binder() : Result<TBindValue>.Failure(_errors);
    }
}

public static class ResultBindAsyncExtensions
{
    public static async Task<Result<TBindValue>> BindAsync<TSource, TBindValue>(
        this Task<Result<TSource>> result,
        Func<TSource, Result<TBindValue>> binder)
    {
        return (await result).Bind(binder);
    }

    public static async Task<Result<TBindValue>> BindAsync<TSource, TBindValue>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<Result<TBindValue>>> binder)
    {
        return await (await result).BindAsync(binder);
    }

    public static async Task<Result<TBindValue>> BindAsync<TBindValue>(
        this Task<Result> result,
        Func<Result<TBindValue>> binder)
    {
        return (await result).Bind(binder);
    }

    public static async Task<Result<TBindValue>> BindAsync<TBindValue>(
        this Task<Result> result,
        Func<Task<Result<TBindValue>>> binder)
    {
        return await (await result).BindAsync(binder);
    }
}