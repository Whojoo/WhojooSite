namespace WhojooSite.Common.Results;

public partial record ResultError
{
    public static ResultError Failure(string code = "General.Failure", string description = "A failure has occurred")
    {
        return new ResultError(code, description);
    }

    public static ResultError BadRequest(string code = "General.Validation", string description = "A validation error has occurred")
    {
        return new ResultError(code, description, ResultStatus.BadRequest);
    }

    public static ResultError NotFound(string code = "General.NotFound", string description = "A not found error has occurred")
    {
        return new ResultError(code, description, ResultStatus.NotFound);
    }

    public static ResultError InternalError(string code = "General.InternalError", string description = "An internal error has occurred")
    {
        return new ResultError(code, description, ResultStatus.InternalError);
    }

    public static ResultError Conflict(string code = "General.Conflict", string description = "A conflict has occurred")
    {
        return new ResultError(code, description, ResultStatus.Conflict);
    }

    public static ResultError Unauthorized(string code = "General.Unauthorized",
        string description = "An unauthorized error has occurred")
    {
        return new ResultError(code, description, ResultStatus.Unauthorized);
    }

    public static ResultError Forbidden(string code = "General.Forbidden", string description = "A forbidden error has occurred")
    {
        return new ResultError(code, description, ResultStatus.Forbidden);
    }
}