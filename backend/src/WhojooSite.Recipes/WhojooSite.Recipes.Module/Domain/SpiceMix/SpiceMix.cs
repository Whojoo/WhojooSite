using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

namespace WhojooSite.Recipes.Module.Domain.SpiceMix;

public sealed class SpiceMix : Entity<SpiceMixId>
{
    private readonly List<Ingredient> _spices = [];
    private readonly string _name = string.Empty;

    public SpiceMix(SpiceMixId id, List<Ingredient> spices, string name) : base(id)
    {
        _spices = spices;
        _name = name;
    }

    private SpiceMix() {}
}