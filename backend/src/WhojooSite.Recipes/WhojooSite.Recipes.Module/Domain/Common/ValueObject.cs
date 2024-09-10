namespace WhojooSite.Recipes.Module.Domain.Common;

public abstract class ValueObject
{
    protected abstract object[] GetEqualityComponents();
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return ((ValueObject)obj)
            .GetEqualityComponents()
            .AsSpan()
            .SequenceEqual(GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(0, (current, obj) => current ^ obj.GetHashCode());
    }
}