using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WhojooSite.Common.Results;

public static class ResultErrorIResultExtensions
{
    public static IResult MapToIResult(this List<ResultError> errors)
    {
        if (errors.Count == 0)
        {
            TypedResults.Problem();
        }

        if (errors.All(error => error.Status == ResultStatus.BadRequest))
        {
            return MapToValidationProblem(errors);
        }

        return MapToProblem(errors[0]);
    }

    private static ValidationProblem MapToValidationProblem(List<ResultError> errors)
    {
        var errorDict = errors
            .GroupBy(error => error.Code, error => error.Description)
            .ToDictionary(group => group.Key, group => group.ToArray());

        return TypedResults.ValidationProblem(errorDict);
    }

    private static IResult MapToProblem(ResultError error)
    {
        return error.Status switch
        {
            ResultStatus.Unspecified => TypedResults.InternalServerError(error.Code),
            ResultStatus.BadRequest => MapToValidationProblem([error]),
            ResultStatus.NotFound => TypedResults.NotFound(error.Code),
            ResultStatus.InternalError => TypedResults.InternalServerError(error.Code),
            ResultStatus.Conflict => TypedResults.Conflict(error.Code),
            ResultStatus.Unauthorized => TypedResults.Unauthorized(),
            ResultStatus.Forbidden => TypedResults.Forbid(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}