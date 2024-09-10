using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class Recipe : Entity<RecipeId>
{
    private readonly string _name = string.Empty;
    private readonly string _description = string.Empty;
    private readonly List<StepId> _stepIds = [];
    private readonly Guid _ownerId = Guid.Empty;

    public Recipe(RecipeId id, string name, string description, Guid ownerId, List<StepId> stepIds)
    : base(id)
    {
        _name = name;
        _description = description;
        _ownerId = ownerId;
        _stepIds = stepIds;
    }

    private Recipe() { }
}