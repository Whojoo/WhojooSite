using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

public class CookbookId(long id) : TypedId<long>(id)
{
    public static CookbookId Empty = new();

    public CookbookId() : this(default)
    {
    }
}