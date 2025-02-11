using Dapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Api;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

using IEndpoint = WhojooSite.Common.Api.IEndpoint;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal sealed class ListRecipesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
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
        RecipesDbConnectionFactory recipesDbConnectionFactory,
        CancellationToken cancellationToken)
    {
        using var connection = recipesDbConnectionFactory.CreateConnection();

        var pageSize = GetPageSize(request.PageSize);
        var currentKey = GetCurrentKey(request.NextKey);

        var recipes = await connection.QueryAsync<ListRecipeItemDto>(
            """
            SELECT "Id", "Name", "Description", "CookbookId"
            FROM "Recipes"
            WHERE "Id" > @Key
            ORDER BY "Id"
            LIMIT @Limit
            """,
            new { Limit = pageSize, Key = currentKey });

        var recipeDtos = recipes.ToList();
        var nextKey = recipeDtos.Count > 0 ? recipeDtos[^1].Id.Value : currentKey;
        var response = new ListRecipesResponse(recipeDtos, nextKey);
        return TypedResults.Ok(response);
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