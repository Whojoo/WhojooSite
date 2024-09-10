using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class SpiceMixIngredient
{
    public SpiceMixId SpiceMixId { get; init; } = SpiceMixId.Empty;
    public RecipeId RecipeId { get; init; } = RecipeId.Empty;
    public double Amount { get; init; }
}