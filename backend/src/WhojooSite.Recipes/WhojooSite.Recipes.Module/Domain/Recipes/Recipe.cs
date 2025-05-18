using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;
using WhojooSite.Recipes.Module.Domain.Cookbooks;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

internal partial class Recipe : Entity<RecipeId>
{
    private readonly List<Ingredient> _ingredients = [];
    private readonly List<SpiceMixIngredient> _spiceMixIngredients = [];
    private readonly List<Ingredient> _spices = [];

    private readonly List<Step> _steps = [];

    public Recipe(
        string name,
        string description,
        OwnerId ownerId,
        List<Step> steps,
        CookbookId cookbookId,
        List<Ingredient> ingredients,
        List<Ingredient> spices,
        List<SpiceMixIngredient> spiceMixIngredients,
        RecipeId? id = null)
        : base(id ?? RecipeId.Empty)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
        CookbookId = cookbookId;
        _steps = steps;
        _ingredients = ingredients;
        _spices = spices;
        _spiceMixIngredients = spiceMixIngredients;
    }

    private Recipe() { }
    public string Name { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public CookbookId CookbookId { get; } = CookbookId.Empty;
    public IReadOnlyList<Step> Steps => _steps.AsReadOnly();
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();
    public IReadOnlyList<Ingredient> Spices => _spices.AsReadOnly();
    public IReadOnlyList<SpiceMixIngredient> SpiceMixIngredients => _spiceMixIngredients.AsReadOnly();
    public OwnerId OwnerId { get; } = OwnerId.Empty;
}