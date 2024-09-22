namespace WhojooSite.Recipes.Module.Domain.Common;

public abstract class TypedId<TIdType>(TIdType value) : IEquatable<TypedId<TIdType>>
    where TIdType : struct
{
    public TIdType Value { get; } = value;

    public bool Equals(TypedId<TIdType>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((TypedId<TIdType>)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}