using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

public sealed class CookbookId(long id = default) : TypedId<long>(id)
{
    public static CookbookId Empty = new();
}