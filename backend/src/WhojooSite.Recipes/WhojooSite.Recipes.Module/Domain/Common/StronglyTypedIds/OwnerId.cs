namespace WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

public class OwnerId(Guid id) : TypedId<Guid>(id)
{
    public static OwnerId Empty { get; } = new();

    public OwnerId() : this(Guid.Empty)
    {
    }
}