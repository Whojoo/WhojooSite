using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public class Step : Entity<StepId>
{
    public string Name { get; } = string.Empty;
    public string Summary { get; } = string.Empty;
    public RecipeId RecipeId { get; } = RecipeId.Empty;

    public Step(
        StepId stepId,
        string name,
        string summary,
        RecipeId recipeId)
        : base(stepId)
    {
        Name = name;
        Summary = summary;
        RecipeId = recipeId;
    }

    private Step() { }
}