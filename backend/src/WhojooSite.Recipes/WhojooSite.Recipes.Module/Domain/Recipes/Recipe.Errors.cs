using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

internal static class RecipeErrors
{
    public static ResultError NameIsRequiredError = ResultError.BadRequest("Recipe.Name", "Name is required");
    public static ResultError DescriptionIsRequiredError = ResultError.BadRequest("Recipe.Description", "Description is required");
}