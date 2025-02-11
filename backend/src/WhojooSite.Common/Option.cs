namespace WhojooSite.Common;

public readonly struct Option<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    public Option() : this(default, false)
    {
    }

    public Option(T? value) : this(value, value is not null)
    {
    }

    private Option(T? value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public TReturn Match<TReturn>(Func<T, TReturn> notNullAction, Func<TReturn> nullAction)
    {
        return _hasValue ? notNullAction(_value!) : nullAction();
    }

    public override string ToString()
    {
        return $"Option<{typeof(T).Name}> {{ {_value?.ToString() ?? "None"} }}";
    }
}

public static class Option
{
    public static Option<T> None<T>() => new();

    public static Option<T> Some<T>(T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new Option<T>(value);
    }

    public static Option<T> Create<T>(T? value) => new(value);
}