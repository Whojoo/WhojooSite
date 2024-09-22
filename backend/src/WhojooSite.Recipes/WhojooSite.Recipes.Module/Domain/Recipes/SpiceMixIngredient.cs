using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public class SpiceMixIngredient
{
    public SpiceMixId SpiceMixId { get; init; } = SpiceMixId.Empty;
    public double Amount { get; init; }
    public string MeasurementUnit { get; init; } = string.Empty;
}