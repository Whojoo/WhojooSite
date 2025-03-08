
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

using WhojooSite.Common.Api;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal sealed class ListRecipesEndpoint
{
    internal static void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/api/recipes", ListRecipesAsync)
            .AddValidation<ListRecipesRequest>()
            .AddRequestLogging<ListRecipesRequest>();
    }

    private sealed class ListRecipesRequest
    {
        [FromQuery]
        public long? NextKey { get; set; }

        [FromQuery]
        public int? PageSize { get; set; }
    }

    private record ListRecipesResponse(List<ListRecipeItemDto> Recipes, long NextKey);

    internal record ListRecipeItemDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    private static async Task<Ok<ListRecipesResponse>> ListRecipesAsync(
        [AsParameters] ListRecipesRequest request,
        RecipesDbContext recipesDbContext,
        CancellationToken cancellationToken)
    {
        var pageSize = GetPageSize(request.PageSize);
        var currentKey = GetCurrentKey(request.NextKey);
        (List<ListRecipeItemDto> recipeDtos, long nextKey) = await RetrieveFromDatabase(
            recipesDbContext,
            pageSize,
            currentKey,
            cancellationToken);

        var response = new ListRecipesResponse(recipeDtos, nextKey);
        return TypedResults.Ok(response);
    }

    private static async Task<(List<ListRecipeItemDto>, long)> RetrieveFromDatabase(
        RecipesDbContext recipesDbContext,
        int pageSize,
        long currentKey,
        CancellationToken cancellationToken)
    {
        var recipeList = await recipesDbContext
            .Recipes
            .Where(recipe => recipe.Id.Value > currentKey)
            .Select(recipe => new ListRecipeItemDto(recipe.Id, recipe.Name, recipe.Description, recipe.CookbookId))
            .Take(pageSize)
            .OrderBy(recipe => recipe.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        var nextKey = recipeList.Count > 0 ? recipeList[^1].Id.Value : currentKey;
        return (recipeList, nextKey);
    }

    private static int GetPageSize(int? pageSize)
    {
        const int defaultPageSize = 10;
        return !pageSize.HasValue ? defaultPageSize : Math.Min(defaultPageSize, pageSize.Value);
    }

    private static long GetCurrentKey(long? nextKey)
    {
        const long defaultNextKey = 0;
        return !nextKey.HasValue ? defaultNextKey : Math.Max(defaultNextKey, nextKey.Value);
    }
}