namespace WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

public class OwnerId(Guid id = default) : TypedId<Guid>(id)
{
    public static OwnerId Empty { get; } = new();
}