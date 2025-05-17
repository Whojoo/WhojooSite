namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public TMatchValue Match<TMatchValue>(Func<T, TMatchValue> successAction, Func<List<ResultError>, TMatchValue> failureAction)
    {
        return IsSuccess ? successAction(_value!) : failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<T, Task<TMatchValue>> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return IsSuccess ? await successAction(_value!) : await failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<T, TMatchValue> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return IsSuccess ? successAction(_value!) : await failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<T, Task<TMatchValue>> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return IsSuccess ? await successAction(_value!) : failureAction(_errors);
    }
}

public partial class Result
{
    public TMatchValue Match<TMatchValue>(Func<TMatchValue> successAction, Func<List<ResultError>, TMatchValue> failureAction)
    {
        return IsSuccess ? successAction() : failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<Task<TMatchValue>> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return IsSuccess ? await successAction() : await failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<TMatchValue> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return IsSuccess ? successAction() : await failureAction(_errors);
    }

    public async Task<TMatchValue> MatchAsync<TMatchValue>(
        Func<Task<TMatchValue>> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return IsSuccess ? await successAction() : failureAction(_errors);
    }
}

public static class ResultAsyncMatchExtensions
{
    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<Result<TSource>> result,
        Func<TSource, TMatchValue> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return (await result).Match(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<TMatchValue>> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<Result<TSource>> result,
        Func<TSource, TMatchValue> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<Result<TSource>> result,
        Func<TSource, Task<TMatchValue>> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TMatchValue>(
        this Task<Result> result,
        Func<TMatchValue> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return (await result).Match(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TMatchValue>(
        this Task<Result> result,
        Func<Task<TMatchValue>> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TMatchValue>(
        this Task<Result> result,
        Func<TMatchValue> successAction,
        Func<List<ResultError>, Task<TMatchValue>> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }

    public static async Task<TMatchValue> MatchAsync<TMatchValue>(
        this Task<Result> result,
        Func<Task<TMatchValue>> successAction,
        Func<List<ResultError>, TMatchValue> failureAction)
    {
        return await (await result).MatchAsync(successAction, failureAction);
    }
}