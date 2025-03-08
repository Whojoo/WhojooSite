using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

using WhojooSite.Common;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal class GetRecipeByIdEndpoint
{
    internal static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/{recipeId}", GetRecipeByIdAsync);
    }

    internal record RecipeDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    private static async Task<Results<Ok<RecipeDto>, NotFound>> GetRecipeByIdAsync(
        RecipeId recipeId,
        RecipesDbContext recipesDbContext)
    {
        var recipeDtoOption = await RetrieveFromDatabaseAsync(recipesDbContext, recipeId);

        return recipeDtoOption.Match<Results<Ok<RecipeDto>, NotFound>>(
            recipeDto => TypedResults.Ok(recipeDto),
            () => TypedResults.NotFound());
    }

    private static async Task<Option<RecipeDto>> RetrieveFromDatabaseAsync(
        RecipesDbContext recipesDbContext,
        RecipeId recipeId)
    {
        var recipe = await recipesDbContext
            .Recipes
            .Where(recipe => recipe.Id == recipeId)
            .Select(recipe => new RecipeDto(
                recipe.Id,
                recipe.Name,
                recipe.Description,
                recipe.CookbookId))
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return Option.Create(recipe);
    }
}