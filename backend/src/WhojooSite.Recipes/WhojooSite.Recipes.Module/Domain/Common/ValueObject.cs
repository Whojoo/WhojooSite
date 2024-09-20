namespace WhojooSite.Recipes.Module.Domain.Common;

public abstract class ValueObject<TValueObject> where TValueObject : ValueObject<TValueObject>
{
    protected abstract bool IsEqualTo(TValueObject other);
    protected abstract int CalculateHashCode();

    public override bool Equals(object? obj)
    {
        return obj is TValueObject other && IsEqualTo(other);
    }

    public override int GetHashCode()
    {
        return CalculateHashCode();
    }

    public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
    {
        return !(left.Equals(right));
    }
}