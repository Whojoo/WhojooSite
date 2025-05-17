namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public Result<TMapValue> Map<TMapValue>(Func<T, TMapValue> mapper)
    {
        return IsSuccess ? Result<TMapValue>.Success(mapper(_value!)) : Result<TMapValue>.Failure(_errors);
    }

    public async Task<Result<TMapValue>> MapAsync<TMapValue>(Func<T, Task<TMapValue>> mapper)
    {
        return IsSuccess ? Result<TMapValue>.Success(await mapper(_value!)) : Result<TMapValue>.Failure(_errors);
    }
}

public partial class Result
{
    public Result<TMapValue> Map<TMapValue>(Func<TMapValue> mapper)
    {
        return IsSuccess ? Result<TMapValue>.Success(mapper()) : Result<TMapValue>.Failure(_errors);
    }

    public async Task<Result<TMapValue>> MapAsync<TMapValue>(Func<Task<TMapValue>> mapper)
    {
        return IsSuccess ? Result<TMapValue>.Success(await mapper()) : Result<TMapValue>.Failure(_errors);
    }
}

public static class ResultMapAsyncExtensions
{
    public static async Task<Result<TMapValue>> MapAsync<TSource, TMapValue>(
        this Task<Result<TSource>> result,
        Func<TSource, TMapValue> mapper)
    {
        return (await result).Map(mapper);
    }

    public static async Task<Result<TMapValue>> MapAsync<TSource, TMapValue>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<TMapValue>> mapper)
    {
        return await (await result).MapAsync(mapper);
    }

    public static async Task<Result<TMapValue>> MapAsync<TMapValue>(
        this Task<Result> result,
        Func<TMapValue> mapper)
    {
        return (await result).Map(mapper);
    }

    public static async Task<Result<TMapValue>> MapAsync<TMapValue>(
        this Task<Result> result,
        Func<Task<TMapValue>> mapper)
    {
        return await (await result).MapAsync(mapper);
    }
}