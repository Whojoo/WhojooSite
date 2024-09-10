using WhojooSite.Recipes.Module.Domain.Common;
using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

public sealed class Step : Entity<StepId>
{
    public List<Ingredient> Ingredients { get; init; } = [];
    public List<Ingredient> Spices { get; init; } = [];

    private readonly string _name = string.Empty;
    private readonly string _summary = string.Empty;
    private readonly string _description = string.Empty;

    public Step(
        StepId stepId,
        string name,
        string summary,
        string description,
        List<Ingredient> ingredients,
        List<Ingredient> spices)
        : base(stepId)
    {
        _name = name;
        _summary = summary;
        _description = description;
        Ingredients = ingredients;
        Spices = spices;
    }

    private Step() { }
}