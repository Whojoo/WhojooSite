using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

public sealed class Cookbook : Entity<CookbookId>
{
    private readonly string _name = string.Empty;
    private readonly List<RecipeId> _recipeIds = [];

    public Cookbook(CookbookId id, string name, List<RecipeId> recipeIds)
        : base(id)
    {
        _name = name;
        _recipeIds = recipeIds;
    }

    private Cookbook() { }
}