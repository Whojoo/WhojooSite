using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public class StepId(long id) : TypedId<long>(id)
{
    public static StepId Empty => new();

    public StepId() : this(default)
    {
    }
}