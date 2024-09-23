using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

internal class SpiceMixIngredient
{
    public SpiceMixId SpiceMixId { get; init; } = SpiceMixId.Empty;
    public double Amount { get; init; }
    public string MeasurementUnit { get; init; } = string.Empty;
}