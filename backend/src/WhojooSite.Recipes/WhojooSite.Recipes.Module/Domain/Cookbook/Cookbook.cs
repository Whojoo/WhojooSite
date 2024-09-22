using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

public class Cookbook : Entity<CookbookId>
{
    public string Name { get; } = string.Empty;
    public IReadOnlyList<RecipeId> RecipeIds => _recipeIds.AsReadOnly();

    private readonly List<RecipeId> _recipeIds = [];

    public Cookbook(CookbookId id, string name, List<RecipeId> recipeIdIds)
        : base(id)
    {
        Name = name;
        _recipeIds = recipeIdIds;
    }

    private Cookbook() { }
}