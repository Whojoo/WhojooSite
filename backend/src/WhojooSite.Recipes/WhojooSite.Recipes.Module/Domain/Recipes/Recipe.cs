using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbook;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class Recipe : Entity<RecipeId>
{
    private readonly string _name = string.Empty;
    private readonly string _description = string.Empty;
    private readonly List<Step> _steps = [];
    private readonly CookbookId _cookbookId;
    private readonly OwnerId _ownerId = OwnerId.Empty;

    public Recipe(
        RecipeId id,
        string name,
        string description,
        OwnerId ownerId,
        List<Step> steps,
        CookbookId cookbookId)
    : base(id)
    {
        _name = name;
        _description = description;
        _ownerId = ownerId;
        _steps = steps;
        _cookbookId = cookbookId;
    }

    private Recipe() { }
}