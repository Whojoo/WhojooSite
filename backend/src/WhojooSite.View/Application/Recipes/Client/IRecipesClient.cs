using WhojooSite.Common;

namespace WhojooSite.View.Application.Recipes.Client;

internal interface IRecipesClient
{
    Task<Result<RecipeDto>> GetRecipeByIdAsync(long id);

    Task<Result<List<RecipeDto>>> ListRecipesAsync(ListRecipesRequest request);
}