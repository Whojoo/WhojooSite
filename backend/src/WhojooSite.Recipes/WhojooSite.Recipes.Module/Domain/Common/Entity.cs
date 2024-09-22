namespace WhojooSite.Recipes.Module.Domain.Common;

public abstract class Entity<TId> where TId : notnull
{
    public TId Id { get; init; } = default!;

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> other && Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected Entity(TId id) => Id = id;

    protected Entity() { }
}