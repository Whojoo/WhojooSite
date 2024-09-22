using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public class RecipeId(long id) : TypedId<long>(id)
{
    public static RecipeId Empty { get; } = new();

    public RecipeId() : this(default)
    {
    }
}