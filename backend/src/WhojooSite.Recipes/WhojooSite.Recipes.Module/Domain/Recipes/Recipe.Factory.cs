using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

using CookbookId = WhojooSite.Recipes.Module.Domain.Cookbooks.CookbookId;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

internal partial class Recipe
{
    public static class Factory
    {
        public static Result<Recipe> CreateSimpleRecipe(string name, string description, CookbookId cookbookId, OwnerId ownerId)
        {
            var result = Result.Success();

            if (string.IsNullOrWhiteSpace(name))
            {
                result.WithError(RecipeErrors.NameIsRequiredError);
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                result.WithError(RecipeErrors.DescriptionIsRequiredError);
            }

            return result.Map(() => new Recipe(
                name,
                description,
                ownerId,
                [],
                cookbookId,
                [],
                [],
                []));
        }
    }
}