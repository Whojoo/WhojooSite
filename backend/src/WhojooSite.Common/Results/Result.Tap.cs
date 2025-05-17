namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public void Tap(Action<T> successAction, Action<List<ResultError>> failureAction)
    {
        if (IsSuccess)
        {
            successAction(_value!);
        }
        else
        {
            failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Func<T, Task> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        if (IsSuccess)
        {
            await successAction(_value!);
        }
        else
        {
            await failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Action<T> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        if (IsSuccess)
        {
            successAction(_value!);
        }
        else
        {
            await failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Func<T, Task> successAction,
        Action<List<ResultError>> failureAction)
    {
        if (IsSuccess)
        {
            await successAction(_value!);
        }
        else
        {
            failureAction(_errors);
        }
    }
}

public partial class Result
{
    public void Tap(Action successAction, Action<List<ResultError>> failureAction)
    {
        if (IsSuccess)
        {
            successAction();
        }
        else
        {
            failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Func<Task> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        if (IsSuccess)
        {
            await successAction();
        }
        else
        {
            await failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Action successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        if (IsSuccess)
        {
            successAction();
        }
        else
        {
            await failureAction(_errors);
        }
    }

    public async Task TapAsync(
        Func<Task> successAction,
        Action<List<ResultError>> failureAction)
    {
        if (IsSuccess)
        {
            await successAction();
        }
        else
        {
            failureAction(_errors);
        }
    }
}

public static class ResultAsyncTapExtensions
{
    public static async Task TapAsync<TSource>(
        this Task<Result<TSource>> result,
        Action<TSource> successAction,
        Action<List<ResultError>> failureAction)
    {
        (await result).Tap(successAction, failureAction);
    }

    public static async Task TapAsync<TSource>(
        this Task<Result<TSource>> result,
        Func<TSource, Task> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }

    public static async Task TapAsync<TSource>(
        this Task<Result<TSource>> result,
        Action<TSource> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }

    public static async Task TapAsync<TSource>(
        this Task<Result<TSource>> result,
        Func<TSource, Task> successAction,
        Action<List<ResultError>> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }

    public static async Task TapAsync(
        this Task<Result> result,
        Action successAction,
        Action<List<ResultError>> failureAction)
    {
        (await result).Tap(successAction, failureAction);
    }

    public static async Task TapAsync(
        this Task<Result> result,
        Func<Task> successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }

    public static async Task TapAsync(
        this Task<Result> result,
        Action successAction,
        Func<List<ResultError>, Task> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }

    public static async Task TapAsync(
        this Task<Result> result,
        Func<Task> successAction,
        Action<List<ResultError>> failureAction)
    {
        await (await result).TapAsync(successAction, failureAction);
    }
}