namespace WhojooSite.Recipes.Module.Domain.Recipes;

public readonly struct RecipeId(long value) : IEquatable<RecipeId>
{
    public long Value { get; } = value;
    public static RecipeId Empty { get; } = new(0);

    public bool Equals(RecipeId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is RecipeId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}