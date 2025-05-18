using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Domain.Cookbooks;

internal class Cookbook : Entity<CookbookId>
{
    private readonly List<RecipeId> _recipeIds = [];

    public Cookbook(string name, List<RecipeId>? recipeIdIds = null, CookbookId? id = null)
        : base(id ?? CookbookId.Empty)
    {
        Name = name;
        _recipeIds = recipeIdIds ?? [];
    }

    private Cookbook() { }
    public string Name { get; } = string.Empty;
    public IReadOnlyList<RecipeId> RecipeIds => _recipeIds.AsReadOnly();
}