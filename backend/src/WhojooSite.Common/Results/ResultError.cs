namespace WhojooSite.Common.Results;

public partial record ResultError(string Code, string Description, ResultStatus Status = ResultStatus.Unspecified);

public enum ResultStatus
{
    Unspecified,
    BadRequest,
    NotFound,
    InternalError,
    Conflict,
    Unauthorized,
    Forbidden
}