using Dapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

using IEndpoint = WhojooSite.Common.Api.IEndpoint;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal class GetRecipeByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/api/recipes/{recipeId}", GetRecipeByIdAsync);
    }

    internal record RecipeDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    private static async Task<Results<Ok<RecipeDto>, NotFound>> GetRecipeByIdAsync(
        RecipeId recipeId,
        RecipesDbConnectionFactory recipesDbConnectionFactory)
    {
        var recipeDtoOption = await RetrieveFromDatabaseAsync(recipesDbConnectionFactory, recipeId);

        return recipeDtoOption.Match<Results<Ok<RecipeDto>, NotFound>>(
            recipeDto => TypedResults.Ok(recipeDto),
            () => TypedResults.NotFound());
    }

    private static async Task<Option<RecipeDto>> RetrieveFromDatabaseAsync(
        RecipesDbConnectionFactory connectionFactory,
        RecipeId recipeId)
    {
        using var connection = connectionFactory.CreateConnection();

        var recipeDto = await connection.QueryFirstOrDefaultAsync<RecipeDto>(
            """
            SELECT "Id", "Name", "Description", "CookbookId" 
            FROM "Recipes" 
            WHERE "Id" = @Id
            """,
            new { Id = recipeId });

        return Option.Create(recipeDto);
    }
}