namespace WhojooSite.Common.Results;

public partial class Result<T>
{
    private readonly List<ResultError> _errors;
    private readonly T? _value;

    internal Result(T? value, List<ResultError> errors)
    {
        _value = value;
        _errors = errors;
    }

    internal Result(ResultError error) : this([error]) { }
    internal Result(List<ResultError> errors) : this(default, errors) { }

    private bool IsSuccess => _errors.Count == 0;

    public void WithError(ResultError error)
    {
        _errors.Add(error);
    }

    public void WithErrors(List<ResultError> errors)
    {
        _errors.AddRange(errors);
    }
}

public partial class Result
{
    private readonly List<ResultError> _errors;

    private Result(List<ResultError> errors)
    {
        _errors = errors;
    }

    private Result(ResultError error) : this([error]) { }

    private bool IsSuccess => _errors.Count == 0;

    public void WithError(ResultError error)
    {
        _errors.Add(error);
    }

    public void WithErrors(List<ResultError> errors)
    {
        _errors.AddRange(errors);
    }
}