using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class RecipeId(long id = default) : TypedId<long>(id)
{
    public static RecipeId Empty { get; } = new();
}