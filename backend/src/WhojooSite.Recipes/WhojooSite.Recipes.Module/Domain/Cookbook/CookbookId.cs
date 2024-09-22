namespace WhojooSite.Recipes.Module.Domain.Cookbook;

public readonly struct CookbookId(long value) : IEquatable<CookbookId>
{
    public long Value { get; } = value;
    public static CookbookId Empty { get; } = new(0);

    public bool Equals(CookbookId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is CookbookId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}