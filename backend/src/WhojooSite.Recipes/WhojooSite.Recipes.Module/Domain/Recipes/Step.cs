using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

internal class Step : Entity<StepId>
{
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
    public string Name { get; } = string.Empty;
    public string Summary { get; } = string.Empty;
    public RecipeId RecipeId { get; } = RecipeId.Empty;
}