using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class StepId(long id = default) : TypedId<long>(id)
{
}