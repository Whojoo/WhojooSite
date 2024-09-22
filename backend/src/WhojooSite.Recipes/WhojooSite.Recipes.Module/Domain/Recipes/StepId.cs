namespace WhojooSite.Recipes.Module.Domain.Recipes;

public readonly struct StepId(long value) : IEquatable<StepId>
{
    public long Value { get; } = value;

    public bool Equals(StepId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is StepId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}