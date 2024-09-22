namespace WhojooSite.Recipes.Module.Domain.SpiceMix;

public readonly struct SpiceMixId(long value) : IEquatable<SpiceMixId>
{
    public long Value { get; } = value;
    public static SpiceMixId Empty { get; } = new(0);

    public bool Equals(SpiceMixId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SpiceMixId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}