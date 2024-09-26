using Ardalis.GuardClauses;

namespace WhojooSite.Common;

public readonly struct Option<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    public static readonly Option<T> None = default;

    public static Option<T> Some(T value)
    {
        Guard.Against.Null(value);
        return new Option<T>(value);
    }

    public Option(T? value)
    {
        _value = value;
        _hasValue = _value is not null;
    }

    public void Match(Action<T> notNullAction, Action nullAction)
    {
        if (_hasValue)
        {
            notNullAction(_value!);
        }
        else
        {
            nullAction();
        }
    }

    public Task MatchAsync(Func<T, Task> notNullAction, Func<Task> nullAction)
    {
        return _hasValue ? notNullAction(_value!) : nullAction();
    }

    public Option<TMappedType> Map<TMappedType>(Func<T, TMappedType> mapFunc)
    {
        return _hasValue ? mapFunc(_value!) : Option<TMappedType>.None;
    }

    public static implicit operator Option<T>(T? value) => new(value);
}