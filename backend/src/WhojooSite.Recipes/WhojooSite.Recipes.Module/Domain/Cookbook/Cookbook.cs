using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

internal class Cookbook : Entity<CookbookId>
{
    private readonly List<RecipeId> _recipeIds = [];

    public Cookbook(CookbookId id, string name, List<RecipeId> recipeIdIds)
        : base(id)
    {
        Name = name;
        _recipeIds = recipeIdIds;
    }

    private Cookbook() { }
    public string Name { get; } = string.Empty;
    public IReadOnlyList<RecipeId> RecipeIds => _recipeIds.AsReadOnly();
}