using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

namespace WhojooSite.Recipes.Module.Domain.SpiceMix;

public class SpiceMix : Entity<SpiceMixId>
{
    public IReadOnlyList<Ingredient> Spices => _spices.AsReadOnly();
    public string Name { get; } = string.Empty;

    private readonly List<Ingredient> _spices = [];

    public SpiceMix(SpiceMixId id, List<Ingredient> spices, string name) : base(id)
    {
        _spices = spices;
        Name = name;
    }

    private SpiceMix() { }
}