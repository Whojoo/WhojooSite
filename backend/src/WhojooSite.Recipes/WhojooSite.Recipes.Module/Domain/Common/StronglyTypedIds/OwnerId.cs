
namespace WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

public readonly struct OwnerId(Guid value) : IEquatable<OwnerId>
{
    public Guid Value { get; } = value;
    public static OwnerId Empty { get; } = new(Guid.Empty);

    public bool Equals(OwnerId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is OwnerId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}