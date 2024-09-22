using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.SpiceMix;

public class SpiceMixId(long id = default) : TypedId<long>(id)
{
    public static SpiceMixId Empty { get; } = new();
}