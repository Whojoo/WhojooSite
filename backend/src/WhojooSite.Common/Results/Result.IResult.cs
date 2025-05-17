using Microsoft.AspNetCore.Http;

namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    public IResult MapToIResult()
    {
        return Match(
            TypedResults.Ok,
            errors => errors.MapToIResult());
    }
}

public partial class Result
{
    public IResult MapToIResult()
    {
        return Match(
            TypedResults.Ok,
            errors => errors.MapToIResult());
    }
}

public static class ResultIResultExtensions
{
    public static async Task<IResult> MapToIResultAsync<T>(this Task<Result<T>> result)
    {
        return (await result).MapToIResult();
    }

    public static async Task<IResult> MapToIResultAsync(this Task<Result> result)
    {
        return (await result).MapToIResult();
    }
}